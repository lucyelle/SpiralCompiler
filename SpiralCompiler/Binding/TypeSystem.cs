using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.Binding;

public static class TypeSystem
{
    public static TypeSymbol Assignable(TypeSymbol left, TypeSymbol right)
    {
        if (!IsAssignableTo(left, right))
        {
            throw new InvalidOperationException($"{right} is not assignable to {left}");
        }
        return left;
    }

    public static void Condition(TypeSymbol type)
    {
        if (!IsBool(type))
        {
            throw new InvalidOperationException($"{type} can not be a condition");
        }
    }

    public static FunctionSymbol ResolveOverload(OverloadSymbol set, ImmutableArray<TypeSymbol> parameterTypes)
    {
        foreach (var func in set.Functions)
        {
            if (MatchesOverload(func, parameterTypes)) return func;
        }
        throw new InvalidOperationException($"no matching overload for {set.Name}");
    }

    private static bool MatchesOverload(FunctionSymbol overload, ImmutableArray<TypeSymbol> parameterTypes)
    {
        var overloadParamTypes = overload.Parameters.Select(p => p.Type);
        foreach (var (overloadParam, passedInParam) in overloadParamTypes.Zip(parameterTypes))
        {
            if (!IsAssignableTo(overloadParam, passedInParam)) return false;
        }
        return true;
    }

    private static bool IsAssignableTo(TypeSymbol left, TypeSymbol right) =>
        right.BaseTypes.Contains(left);

    private static bool IsBool(TypeSymbol type) => type == BuiltInTypeSymbol.Bool;
}
