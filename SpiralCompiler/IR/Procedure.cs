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

    private readonly FunctionSymbol symbol;

    private int registerCount;

    public Procedure(FunctionSymbol symbol)
    {
        this.symbol = symbol;
    }

    public override string ToString() => $"""
        proc {symbol.Name}({string.Join(", ", Parameters.Values)}):
        {string.Join("\n", BasicBlocks)}
        """;

    public Parameter DefineParameter(ParameterSymbol symbol)
    {
        if (!Parameters.TryGetValue(symbol, out var param))
        {
            param = new(symbol);
            Parameters.Add(symbol, param);
        }
        return param;
    }

    public Local DefineLocal(LocalVariableSymbol symbol)
    {
        if (!Locals.TryGetValue(symbol, out var local))
        {
            local = new(symbol);
            Locals.Add(symbol, local);
        }
        return local;
    }

    public Register AllocateRegister() => new(registerCount++);
}
