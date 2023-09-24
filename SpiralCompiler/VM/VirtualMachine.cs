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
            var stk = frame.ComputationStack;
            var instr = byteCode.Instructions[IP];

            int IntOperand(int index = 0) => (int)instr!.Operands[index]!;

            switch (instr.Opcode)
            {
                case OpCode.Add:
                {
                    var left = stk.Pop();
                    var right = stk.Pop();
                    stk.Push(left + right);
                    IP++;
                    break;
                }
                case OpCode.Stackalloc:
                {
                    var nLocals = IntOperand();
                    frame.Locals = new dynamic[nLocals];
                    IP++;
                    break;
                }
                case OpCode.PushConst:
                {
                    stk.Push(IntOperand());
                    IP++;
                    break;
                }
                case OpCode.PushParam:
                {
                    var paramIndex = IntOperand();
                    stk.Push(frame.Args[paramIndex]);
                    IP++;
                    break;
                }
                case OpCode.PushLocal:
                {
                    var localIndex = IntOperand();
                    stk.Push(frame.Locals[localIndex]);
                    IP++;
                    break;
                }
                case OpCode.Dup:
                {
                    stk.Push(stk.Peek());
                    IP++;
                    break;
                }
                case OpCode.Pop:
                {
                    stk.Pop();
                    IP++;
                    break;
                }
                case OpCode.StoreLocal:
                {
                    var localIndex = IntOperand();
                    frame.Locals[localIndex] = stk.Pop();
                    IP++;
                    break;
                }
                case OpCode.Return_0:
                case OpCode.Return_1:
                {
                    var returnAddr = (int)frame.ReturnAddress;
                    var returnedValue = instr.Opcode == OpCode.Return_1
                        ? stk.Pop()
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
