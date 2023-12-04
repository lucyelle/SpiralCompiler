using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Binding;

public static class TypeSystem
{
    public static TypeSymbol Assignable(SyntaxNode syntax, TypeSymbol left, TypeSymbol right, List<ErrorMessage> errors)
    {
        if (left == BuiltInTypeSymbol.Error || right == BuiltInTypeSymbol.Error)
        {
            return BuiltInTypeSymbol.Error;
        }
        if (!IsAssignableTo(left, right))
        {
            errors.Add(new ErrorMessage($"cannot assign {right} to {left}", syntax));
            return BuiltInTypeSymbol.Error;
        }
        return left;
    }

    public static void Condition(SyntaxNode syntax, TypeSymbol type, List<ErrorMessage> errors)
    {
        if (type == BuiltInTypeSymbol.Error)
        {
            return;
        }
        if (!IsBool(type))
        {
            errors.Add(new ErrorMessage($"condition must be bool expression, but got {type}", syntax));
        }
    }

    public static FunctionSymbol ResolveOverload(
        SyntaxNode syntax,
        OverloadSymbol set,
        ImmutableArray<TypeSymbol> parameterTypes,
        List<ErrorMessage> errors)
    {
        foreach (var func in set.Functions)
        {
            if (MatchesOverload(func, parameterTypes)) return func;
        }

        errors.Add(new ErrorMessage($"No overload of {set.Name} matches the argument types", syntax));
        return new ErrorFunctionSymbol(set.Name, parameterTypes.Length);
    }

    private static bool MatchesOverload(FunctionSymbol overload, ImmutableArray<TypeSymbol> parameterTypes)
    {
        if (overload.Parameters.Length != parameterTypes.Length) return false;

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
