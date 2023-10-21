using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.VM;

public readonly record struct VTable(
    InterfaceSymbol Interface,
    ImmutableDictionary<FunctionSymbol, int> Addresses);

public sealed record class TypeInfo(
    TypeSymbol Type,
    ImmutableArray<VTable> VTables,
    ImmutableArray<dynamic?> DefaultFieldValues)
{
    public override string ToString() => Type.ToString();
}

public sealed class RuntimeObject
{
    public TypeInfo TypeInfo { get; }
    public List<dynamic?> Fields { get; }

    public RuntimeObject(TypeInfo typeInfo)
    {
        TypeInfo = typeInfo;
        Fields = typeInfo.DefaultFieldValues.ToList();
    }
}
