using SpiralCompiler.Syntax;

namespace SpiralCompiler.CodeGen;
public sealed class Interpreter
{
    private readonly Stack<StackFrame> callStack = new();
    private Instruction[] code;

    public void Run(Statement ast)
    {
        var ip = 0;
        var compiledCode = Compiler.Compile(ast);

        Console.WriteLine(compiledCode);

        var mainFunc = compiledCode.FuncDefs.Where(f => f.Symbol.Name == "main").Single();
        mainFunc.Address = ip;

        callStack.Push(new StackFrame
        {
            ReturnAddress = ip
        });

        code = FlattenModule(compiledCode).ToArray();

        while (callStack.Count > 0)
        {
            var frame = callStack.Peek();
            var instruction = code[ip];

            switch (instruction)
            {
                case Instruction.Call call:

                    if (call.Func is Operand.BuiltInFunction func)
                    {
                        var args = call.Args.Select(GetValue).ToArray();
                        var result = func.Delegate.DynamicInvoke(args);

                        frame.Registers[call.Target] = result;
                        ip++;
                    }

                    else
                    {
                        var callFunc = ((Operand.Function)call.Func).FuncDef;
                        var callFrame = new StackFrame
                        {
                            Variables = callFunc.Params.Zip(call.Args.Select(GetValue)).ToDictionary(pair => pair.First, pair => pair.Second),
                            ReturnAddress = ip,
                            ReturnRegister = call.Target,
                        };
                        callStack.Push(callFrame);
                        ip = callFunc.Address;
                    }

                    break;

                case Instruction.Load load:

                    frame.Registers[load.Target] = frame.Variables[load.Source];
                    ip++;
                    break;

                case Instruction.Store store:

                    if (!frame.Variables.ContainsKey(store.Target))
                    {
                        frame.Variables.Add(store.Target, GetValue(store.Source));
                    }
                    else
                    {
                        frame.Variables[store.Target] = GetValue(store.Source);
                    }
                    ip++;
                    break;

                case Instruction.Return r:

                    if (r.Value is null)
                    {
                        ip = frame.ReturnAddress + 1;
                        callStack.Pop();
                        break;
                    }
                    var returnValue = GetValue(r.Value);
                    ip = frame.ReturnAddress + 1;
                    var returnReg = frame.ReturnRegister;

                    callStack.Pop();
                    frame = callStack.Peek();
                    frame.Registers[returnReg!] = returnValue;
                    break;

                case Instruction.Goto instr:

                    ip = instr.Label.Address;
                    break;

                case Instruction.GotoIf gotoIf:

                    // Convert condition to bool
                    var conditionValue = (bool)GetValue(gotoIf.Condition)!;

                    // Evaluate condition
                    if (conditionValue)
                    {
                        ip = gotoIf.Then.Address;
                    }
                    else
                    {
                        ip = gotoIf.Else.Address;
                    }
                    break;

                case Instruction.Arithmetic instr:

                    var leftValue = GetValue(instr.Left);
                    var rightValue = GetValue(instr.Right);

                    if (leftValue is null || rightValue is null)
                    {
                        throw new InvalidOperationException();
                    }

                    dynamic left = leftValue;
                    dynamic right = rightValue;

                    frame.Registers![instr.Target] = instr.Op switch
                    {
                        ArithmeticOp.Add => left + right,
                        ArithmeticOp.Subtract => left - right,
                        ArithmeticOp.Multiply => left * right,
                        ArithmeticOp.Divide => left / right,
                        ArithmeticOp.Equals => left == right,
                        ArithmeticOp.Less => left < right,
                        ArithmeticOp.Greater => left > right,
                        ArithmeticOp.Modulo => left % right,
                        ArithmeticOp.NotEqual => left != right,
                        _ => throw new NotImplementedException()
                    };
                    ip++;
                    break;
            }
        }
    }

    private object? GetValue(Operand operand) => operand switch
    {
        Operand.Register r => callStack.Peek().Registers[r],
        Operand.Constant c => c.Value,
        _ => throw new InvalidOperationException(),
    };

    public static List<Instruction> FlattenModule(Module module)
    {
        var result = new List<Instruction>();
        var address = 0;

        foreach (var funcDef in module.FuncDefs)
        {
            funcDef.Address = address;
            foreach (var block in funcDef.Body)
            {
                block.Label.Address = address;
                foreach (var instr in block.Instructions)
                {
                    result.Add(instr);
                    address++;
                }
            }
        }
        return result;
    }
}
