using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.IR;

public interface IOperand
{
    public string ToOperandString();
}

public sealed record class Register(int Index) : IOperand
{
    public override string ToString() => $"r_{Index}";

    public string ToOperandString() => ToString();
}

public sealed record class Local(LocalVariableSymbol Symbol) : IOperand
{
    public override string ToString() => Symbol.Name;

    public string ToOperandString() => ToString();
}

public sealed record class Parameter(ParameterSymbol Symbol) : IOperand
{
    public override string ToString() => $"{Symbol.Name}: {Symbol.Type}";

    public string ToOperandString() => Symbol.Name;
}
