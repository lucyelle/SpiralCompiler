using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.VM;

public sealed class VirtualMachine
{
    private readonly ByteCode byteCode;

    private readonly Stack<StackFrame> callStack = new();

    public VirtualMachine(ByteCode byteCode)
    {
        this.byteCode = byteCode;
    }

    public dynamic? Call(int address, params object[] args)
    {
        callStack.Push(new StackFrame()
        {
            Args = args,
            ReturnAddress = -1,
        });
        var IP = address;
        while (true)
        {
            var frame = callStack.Peek();
            var instr = byteCode.Instructions[IP];

            switch (instr.Opcode)
            {
                case OpCode.PushConst:
                {
                    frame.ComputationStack.Push(instr.Operands[0]);
                    IP++;
                    break;
                }
                case OpCode.PushParam:
                {
                    var paramIndex = (int)instr.Operands[0]!;
                    frame.ComputationStack.Push(frame.Args[paramIndex]);
                    IP++;
                    break;
                }
                case OpCode.Return_0:
                case OpCode.Return_1:
                {
                    var returnAddr = (int)frame.ReturnAddress;
                    var returnedValue = instr.Opcode == OpCode.Return_1
                        ? frame.ComputationStack.Pop()
                        : null;
                    callStack.Pop();
                    IP = returnAddr;
                    if (returnAddr == -1)
                    {
                        // Done
                        return returnedValue;
                    }
                    else if (instr.Opcode == OpCode.Return_1)
                    {
                        callStack.Peek().ComputationStack.Push(returnedValue);
                    }
                    break;
                }
                default:
                    throw new InvalidOperationException($"unknown instruction {instr.Opcode}");
            }
        }
    }
}
