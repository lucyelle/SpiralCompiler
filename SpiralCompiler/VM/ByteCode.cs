using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.VM;

public sealed record class ByteCode(ImmutableArray<Instruction> Instructions)
{
    public override string ToString() => string.Join(Environment.NewLine, Instructions);
}

public sealed record class Instruction(OpCode Opcode, object?[] Operands)
{
    public override string ToString() => $"{Opcode} {string.Join(", ", Operands)}";
}

public enum OpCode
{
    // Allocates the given amount of locals on the stack
    Stackalloc,
    // Return nothing
    Return_0,
    // Return top operand on the stack
    Return_1,
    // Pushes the parameter value onto the stack
    PushConst,
    // Pushes the local at the given index
    PushLocal,
    // Pushes the parameter at the given index
    PushParam,
    // Duplicates the top stack element
    Dup,
    // Pops off a single value
    Pop,
    // Stores popped off value in the given local
    StoreLocal,
    // Stores popped off value in the given arg
    StoreArg,
    // Adds the top 2 values
    Add,
}
