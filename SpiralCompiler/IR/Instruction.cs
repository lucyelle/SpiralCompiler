using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.IR;

public abstract class Instruction
{
}

public sealed class ReturnInstruction : Instruction
{
    public IOperand? Value { get; set; }

    public override string ToString() => $"return {Value?.ToOperandString()}";
}

public sealed class LoadInstruction : Instruction
{
    public required Register Target { get; set; }

    public required IOperand Source { get; set; }

    public override string ToString() => $"{Target.ToOperandString()} := load {Source.ToOperandString()}";
}
