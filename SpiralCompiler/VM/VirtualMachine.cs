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
                default:
                    throw new InvalidOperationException($"unknown instruction {instr.Opcode}");
            }
        }
    }
}
