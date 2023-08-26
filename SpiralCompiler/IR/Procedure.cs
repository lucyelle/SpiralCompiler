using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.IR;

public sealed class Procedure
{
    public List<BasicBlock> BasicBlocks { get; } = new();

    public Dictionary<ParameterSymbol, Parameter> Parameters { get; } = new();

    public Dictionary<LocalVariableSymbol, Local> Locals { get; } = new();

    public override string ToString() => $"""
        proc ???({string.Join(", ", Parameters.Values)}):
        {string.Join("\n", BasicBlocks)}
        """;

    public Parameter DefineParameter(ParameterSymbol symbol)
    {
        if (!Parameters.TryGetValue(symbol, out var param))
        {
            param = new();
            Parameters.Add(symbol, param);
        }
        return param;
    }

    public Local DefineLocal(LocalVariableSymbol symbol)
    {
        if (!Locals.TryGetValue(symbol, out var local))
        {
            local = new();
            Locals.Add(symbol, local);
        }
        return local;
    }
}
