using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.IR;

public sealed class Assembly
{
    public Dictionary<FunctionSymbol, Procedure> Procedures { get; } = new();
}
