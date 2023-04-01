using SpiralCompiler.Semantics;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.CodeGen;
public class Compiler : AstVisitorBase<Operand>
{
    private FunctionDef currentFuncDef = null!;
    private BasicBlock currentBasicBlock = null!;
    private int registerIndex = 0;
    private Dictionary<Symbol, Operand.Function> functionDefs = new();

    private void CreateBasicBlock(Label label)
    {
        currentBasicBlock = new BasicBlock(label, new());
        currentFuncDef.Body.Add(currentBasicBlock);
    }

    private void WriteInstruction(Instruction instruction) => currentBasicBlock.Instructions.Add(instruction);

    private Operand.Register CreateRegister()
    {
        var register = new Operand.Register(registerIndex);
        registerIndex++;
        return register;
    }

    protected override Operand? VisitIfStatement(Statement.If node)
    {
        var condition = VisitExpression(node.Condition) ?? throw new InvalidOperationException();

        var thenLabel = new Label("then");
        var elseLabel = new Label("else");
        var endifLabel = new Label("endif");

        WriteInstruction(new Instruction.GotoIf(condition, thenLabel, elseLabel));

        CreateBasicBlock(thenLabel);
        VisitStatement(node.Then);
        WriteInstruction(new Instruction.Goto(endifLabel));

        CreateBasicBlock(elseLabel);
        if (node.Else is not null)
        {
            VisitStatement(node.Else);
        }
        WriteInstruction(new Instruction.Goto(endifLabel));

        CreateBasicBlock(endifLabel);

        return null;
    }

    protected override Operand? VisitWhileStatement(Statement.While node)
    {
        var whileLabel = new Label("while");
        var bodyLabel = new Label("body");
        var breakLabel = new Label("break");

        WriteInstruction(new Instruction.Goto(whileLabel));
        CreateBasicBlock(whileLabel);

        var condition = VisitExpression(node.Condition) ?? throw new InvalidOperationException(); ;

        WriteInstruction(new Instruction.GotoIf(condition, bodyLabel, breakLabel));

        CreateBasicBlock(bodyLabel);
        VisitStatement(node.Body);
        WriteInstruction(new Instruction.Goto(whileLabel));

        CreateBasicBlock(breakLabel);

        return null;
    }

    protected override Operand? VisitFunctionDefStatement(Statement.FunctionDef node)
    {
        var funcLabel = new Label("func");
        currentFuncDef = new FunctionDef(node.Symbol!, new(), new(), new());
        CreateBasicBlock(funcLabel);

        foreach (var param in node.Params)
        {
            currentFuncDef.Params.Add((Operand.Local)VisitParameter(param)!);
        }

        VisitStatement(node.Body);

        functionDefs.Add(node.Symbol!, new Operand.Function(currentFuncDef));

        return null;
    }

    protected override Operand? VisitReturnStatement(Statement.Return node)
    {
        Operand? value = null;
        if (node.Expression is not null)
        {
            value = VisitExpression(node.Expression);
        }

        WriteInstruction(new Instruction.Return(value));

        return null;
    }

    protected override Operand? VisitFunctionCallExpression(Expression.FunctionCall node)
    {
        var parameters = new List<Operand>();

        foreach (var param in node.Params)
        {
            parameters.Add(VisitExpression(param)!);
        }

        var def = VisitExpression(node.Function);
        var register = CreateRegister();

        WriteInstruction(new Instruction.Call(register, def!, parameters));

        return register;
    }

    protected override Operand? VisitBinaryExpression(Expression.Binary node)
    {
        if (node.Op == BinOp.Assign)
        {
            return CompileAssignment(node);
        }

        var left = VisitExpression(node.Left) ?? throw new InvalidOperationException();
        var right = VisitExpression(node.Right) ?? throw new InvalidOperationException();
        var register = CreateRegister();

        Instruction instruction = node.Op switch
        {
            BinOp.Add => new Instruction.Arithmetic(register, ArithmeticOp.Add, left, right),
            BinOp.Substract => new Instruction.Arithmetic(register, ArithmeticOp.Subtract, left, right),
            BinOp.Multiply => new Instruction.Arithmetic(register, ArithmeticOp.Multiply, left, right),
            BinOp.Divide => new Instruction.Arithmetic(register, ArithmeticOp.Divide, left, right),
            BinOp.Equals => new Instruction.Arithmetic(register, ArithmeticOp.Equals, left, right),
            BinOp.NotEqual => throw new NotImplementedException(),
            BinOp.Less => new Instruction.Arithmetic(register, ArithmeticOp.Less, left, right),
            BinOp.Greater => new Instruction.Arithmetic(register, ArithmeticOp.Greater, left, right),
            BinOp.LessEquals => throw new NotImplementedException(),
            BinOp.GreaterEquals => throw new NotImplementedException(),
            BinOp.Or => throw new NotImplementedException(),
            BinOp.And => throw new NotImplementedException(),
            BinOp.AddAssign => throw new NotImplementedException(),
            BinOp.SubtractAssign => throw new NotImplementedException(),
            BinOp.MultiplyAssign => throw new NotImplementedException(),
            BinOp.DivideAssign => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        };

        WriteInstruction(instruction);

        return register;
    }

    private Operand? CompileAssignment(Expression.Binary node)
    {
        var right = VisitExpression(node.Right)!;
        var left = CompileLeftValue(node.Left);

        WriteInstruction(new Instruction.Store(left, right));
        return right;
    }

    private Operand.Local CompileLeftValue(Expression node)
    {
        switch (node)
        {
            case Expression.Identifier id:
                return new Operand.Local(id.Symbol!);
            default:
                throw new ArgumentOutOfRangeException(nameof(node));
        }
    }

    protected override Operand? VisitIdentifierExpression(Expression.Identifier node)
    {
        if (node.Symbol is Symbol.Function func)
        {
            return functionDefs[func];
        }
        else if (node.Symbol is Symbol.Variable var)
        {
            var r0 = CreateRegister();
            var local = new Operand.Local(var);
            WriteInstruction(new Instruction.Load(r0, local));
            return r0;
        }
        else
        {
            // TODO
            throw new NotImplementedException();
        }
    }

    protected override Operand? VisitUnaryPostExpression(Expression.UnaryPost node)
    {
        var left = (Operand.Local)(VisitExpression(node.Left) ?? throw new InvalidOperationException());

        var r0 = CreateRegister();
        WriteInstruction(new Instruction.Load(r0, left));
        var r1 = CreateRegister();

        Instruction instruction = node.Op switch
        {
            // r0 := load x
            // r1 := r0 + 1
            // store x, r1
            UnOpPost.Increment => new Instruction.Arithmetic(r1, ArithmeticOp.Add, r0, new Operand.Constant(1)),
            UnOpPost.Decrement => new Instruction.Arithmetic(r1, ArithmeticOp.Add, r0, new Operand.Constant(-1)),
            _ => throw new NotImplementedException()
        };

        WriteInstruction(instruction);
        WriteInstruction(new Instruction.Store(left, r1));

        return r0;
    }

    protected override Operand? VisitUnaryPreExpression(Expression.UnaryPre node)
    {
        var right = (Operand.Local)(VisitExpression(node.Right) ?? throw new InvalidOperationException());

        var r0 = CreateRegister();
        WriteInstruction(new Instruction.Load(r0, right));
        var r1 = CreateRegister();

        Instruction instruction = node.Op switch
        {
            UnOpPre.Increment => new Instruction.Arithmetic(r1, ArithmeticOp.Add, r0, new Operand.Constant(1)),
            UnOpPre.Decrement => throw new NotImplementedException(),
            UnOpPre.Minus => new Instruction.Arithmetic(r1, ArithmeticOp.Multiply, new Operand.Constant(-1), right),
            UnOpPre.Plus => new Instruction.Arithmetic(r1, ArithmeticOp.Multiply, new Operand.Constant(1), right),
            UnOpPre.Not => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        };

        WriteInstruction(instruction);
        WriteInstruction(new Instruction.Store(right, r1));

        return r1;
    }

    protected override Operand? VisitVarStatement(Statement.Var node)
    {
        var local = new Operand.Local(node.Symbol!);
        currentFuncDef.Locals.Add(local);

        if (node.Value is not null)
        {
            var value = VisitExpression(node.Value);
            WriteInstruction(new Instruction.Store(local, value!));
        }

        return null;
    }

    protected override Operand? VisitParameter(Parameter param)
    {
        if (param.Symbol is not null)
        {
            return new Operand.Local(param.Symbol);
        }
        return null;
    }

    // TODO: for
}
