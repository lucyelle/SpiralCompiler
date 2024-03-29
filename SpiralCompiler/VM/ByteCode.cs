using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.VM;

public sealed record class ByteCode(
    int GlobalCount,
    ImmutableDictionary<int, FunctionSymbol> AddressesToFunctions,
    ImmutableArray<Instruction> Instructions)
{
    public int GetAddress(string name) => GetAddress(f => f.Name == name);
    public int GetAddress(FunctionSymbol func) => GetAddress(f => f == func);
    public int GetAddress(Predicate<FunctionSymbol> predicate) => AddressesToFunctions.First(kvp => predicate(kvp.Value)).Key;

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var (instr, index) in Instructions.Select((instr, index) => (instr, index)))
        {
            if (AddressesToFunctions.TryGetValue(index, out var func))
            {
                var typePfx = func.ContainingSymbol is TypeSymbol type
                    ? $"{type}::"
                    : string.Empty;
                var paramTypes = func.Parameters.Select(p => p.Type.ToString());
                if (func.IsInstance) paramTypes = paramTypes.Prepend("this");
                sb.AppendLine($"func {typePfx}{func.Name}({string.Join(", ", paramTypes)}) -> {func.ReturnType}:");
            }
            sb.AppendLine($"  {index}: {instr}");
        }
        return sb.ToString();
    }
}

public sealed record class Instruction(OpCode Opcode, object?[] Operands)
{
    public override string ToString() => $"{Opcode} {string.Join(", ", Operands)}";
}

public enum OpCode
{
    // Allocates the given amount of locals on the stack
    Stackalloc,
    // Return top operand on the stack
    Return,
    // Pushes the parameter value onto the stack
    PushConst,
    // Pushes the global at the given index
    PushGlobal,
    // Pushes the local at the given index
    PushLocal,
    // Pushes the parameter at the given index
    PushParam,
    // Pushes the field at the given index of the top value
    PushField,
    // Duplicates the top stack element
    Dup,
    // Swaps the top 2 elements
    Swap,
    // Pops off a single value
    Pop,
    // Stores popped off value in the given global
    StoreGlobal,
    // Stores popped off value in the given local
    StoreLocal,
    // Stores popped off value in the given arg
    StoreArg,
    // Stores popped off value at field of top
    StoreField,
    // Adds the top 2 values
    Add,
    // Subtracts the top 2 values
    Sub,
    // Multiplies the top 2 values
    Mul,
    // Divides the top 2 values
    Div,
    // Modulo between the top 2 values
    Mod,
    // Less-than compares the top 2 values
    Less,
    // Equals compares the top 2 values
    Equals,
    // Negates the value
    Not,
    // Call a regular method
    Call,
    // Call a virtual method
    CallVirt,
    // Calls an intrinsic method
    CallInt,
    // Unconditional absolute jump
    Jmp,
    // Conditional absolute jump
    JmpIf,
    // Object instantiation
    NewObj,
    // Array element access
    ElementAt,
}
