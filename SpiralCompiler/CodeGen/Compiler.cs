using System.Runtime.CompilerServices;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.CodeGen;
public class Compiler : AstVisitorBase<Operand>
{
    private FunctionDef currentFuncDef = null!;
    private BasicBlock currentBasicBlock = null!;

    private void CreateBasicBlock(Label label)
    {
        currentBasicBlock = new BasicBlock(label, new());
        currentFuncDef.Body.Add(currentBasicBlock);
    }

    private void WriteInstruction(Instruction instruction) => currentBasicBlock.Instructions.Add(instruction);

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
        var returnLabel = new Label("return");
        CreateBasicBlock(funcLabel);

        var parameters = new List<Operand>();

        foreach (var param in node.Params)
        {
            parameters.Add(VisitParameter(param)!);
        }

        var body = VisitStatement(node.Body);

        // Locals?

        currentFuncDef = new FunctionDef(parameters, ..., body);
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
}
