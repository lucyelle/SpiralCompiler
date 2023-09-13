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

    public void Run()
    {
        // TODO: temporary
        callStack.Push(new StackFrame());
        var IP = 0;
        while (callStack.TryPeek(out var frame))
        {
            var instr = byteCode.Instructions[IP];

            switch (instr.Opcode)
            {
                case OpCode.Load:
                    var targetIndex = (int)instr.Operands[0];
                    var sourceIndex = (int)instr.Operands[1];

                    frame.Locals[targetIndex] = frame.Locals[sourceIndex];
                    IP++;
                    break;
                case OpCode.Return:
                    IP = frame.ReturnAddress;
                    callStack.Pop();
                    break;
                case OpCode.ReturnLocal:
                    throw new NotImplementedException();
                    break;
                default: throw new InvalidOperationException($"unknown instruction {instr.Opcode}");
            }
        }
    }
}
