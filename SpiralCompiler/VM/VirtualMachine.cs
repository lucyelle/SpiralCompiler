using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.VM;

public sealed class VirtualMachine
{
    private readonly ByteCode byteCode;

    private readonly dynamic?[] globals;
    private readonly Stack<StackFrame> callStack = new();

    public VirtualMachine(ByteCode byteCode)
    {
        this.byteCode = byteCode;
        globals = new dynamic?[byteCode.GlobalCount];
    }

    public dynamic? Call(int address, params object?[] args)
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
                    var right = stk.Pop();
                    var left = stk.Pop();
                    stk.Push(left + right);
                    IP++;
                    break;
                }
                case OpCode.Sub:
                {
                    var right = stk.Pop();
                    var left = stk.Pop();
                    stk.Push(left - right);
                    IP++;
                    break;
                }
                case OpCode.Mul:
                {
                    var right = stk.Pop();
                    var left = stk.Pop();
                    stk.Push(left * right);
                    IP++;
                    break;
                }
                case OpCode.Mod:
                {
                    var right = stk.Pop();
                    var left = stk.Pop();
                    stk.Push(left % right);
                    IP++;
                    break;
                }
                case OpCode.Div:
                {
                    var right = stk.Pop();
                    var left = stk.Pop();
                    if (left is int i1 && right is int i2)
                    {
                        // Integer divide
                        stk.Push(i1 / i2);
                    }
                    else
                    {
                        stk.Push(left / right);
                    }
                    IP++;
                    break;
                }
                case OpCode.Less:
                {
                    var right = stk.Pop();
                    var left = stk.Pop();
                    stk.Push(left < right);
                    IP++;
                    break;
                }
                case OpCode.Equals:
                {
                    var right = stk.Pop();
                    var left = stk.Pop();
                    stk.Push(left == right);
                    IP++;
                    break;
                }
                case OpCode.Not:
                {
                    var sub = stk.Pop();
                    stk.Push(!sub);
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
                    stk.Push(instr.Operands[0]);
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
                case OpCode.PushGlobal:
                {
                    var globalIndex = IntOperand();
                    stk.Push(globals[globalIndex]);
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
                case OpCode.PushField:
                {
                    var fieldIdx = IntOperand();
                    var obj = (RuntimeObject)stk.Pop()!;
                    stk.Push(obj.Fields[fieldIdx]);
                    IP++;
                    break;
                }
                case OpCode.Dup:
                {
                    stk.Push(stk.Peek());
                    IP++;
                    break;
                }
                case OpCode.Swap:
                {
                    var v1 = stk.Pop();
                    var v2 = stk.Pop();
                    stk.Push(v1);
                    stk.Push(v2);
                    IP++;
                    break;
                }
                case OpCode.Pop:
                {
                    stk.Pop();
                    IP++;
                    break;
                }
                case OpCode.StoreGlobal:
                {
                    var globalIndex = IntOperand();
                    globals[globalIndex] = stk.Pop();
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
                case OpCode.StoreField:
                {
                    var obj = (RuntimeObject)stk.Pop()!;
                    var value = stk.Pop();
                    var fieldIdx = IntOperand();
                    obj.Fields[fieldIdx] = value;
                    IP++;
                    break;
                }
                case OpCode.Jmp:
                {
                    IP = IntOperand();
                    break;
                }
                case OpCode.JmpIf:
                {
                    if (stk.Pop()) IP = IntOperand();
                    else IP++;
                    break;
                }
                case OpCode.Return:
                {
                    var returnAddr = frame.ReturnAddress;
                    var returnedValue = stk.Pop();
                    callStack.Pop();
                    IP = returnAddr;
                    if (returnAddr == -1)
                    {
                        // Done
                        return returnedValue;
                    }
                    callStack.Peek().ComputationStack.Push(returnedValue);
                    break;
                }
                case OpCode.Call:
                {
                    var calledAddress = IntOperand(0);
                    var argc = IntOperand(1);
                    // Pop off args
                    var argv = new List<dynamic?>();
                    for (var i = 0; i < argc; i++) argv.Add(stk.Pop());
                    // Reverse
                    argv.Reverse();
                    // Push new stack
                    callStack.Push(new()
                    {
                        Args = argv.ToArray(),
                        ReturnAddress = IP + 1,
                    });
                    IP = calledAddress;
                    break;
                }
                case OpCode.CallVirt:
                {
                    var calledFunc = (FunctionSymbol)instr.Operands[0]!;
                    var argc = IntOperand(1);
                    // Pop off args
                    var argv = new List<dynamic?>();
                    for (var i = 0; i < argc; i++) argv.Add(stk.Pop());
                    // Reverse
                    argv.Reverse();
                    // Look up address in vtable
                    var receiver = (RuntimeObject)argv[0]!;
                    var calledAddress = receiver.TypeInfo.VTables
                        .Single(vt => vt.Interface == calledFunc.OriginatingInterface)
                        .Addresses[calledFunc];
                    // Push new stack
                    callStack.Push(new()
                    {
                        Args = argv.ToArray(),
                        ReturnAddress = IP + 1,
                    });
                    IP = calledAddress;
                    break;
                }
                case OpCode.CallInt:
                {
                    var called = (Func<dynamic[], dynamic>)instr.Operands[0]!;
                    var argc = IntOperand(1);
                    // Pop off args
                    var argv = new List<dynamic>();
                    for (var i = 0; i < argc; i++) argv.Add(stk.Pop());
                    // Reverse, call, push result
                    argv.Reverse();
                    var result = called(argv.ToArray());
                    stk.Push(result);
                    ++IP;
                    break;
                }
                case OpCode.NewObj:
                {
                    var instantiated = (TypeInfo)instr.Operands[0]!;
                    var obj = new RuntimeObject(instantiated);
                    stk.Push(obj);
                    ++IP;
                    break;
                }
                case OpCode.ElementAt:
                {
                    var index = stk.Pop()!;
                    var array = stk.Pop()!;
                    var result = array[index];
                    if (instr.Operands.Length > 0 && (bool)instr.Operands[0]!)
                    {
                        // This is how we annotate string conversion
                        result = result.ToString();
                    }
                    stk.Push(result);
                    ++IP;
                    break;
                }
                default:
                    throw new InvalidOperationException($"unknown instruction {instr.Opcode}");
            }
        }
    }
}
