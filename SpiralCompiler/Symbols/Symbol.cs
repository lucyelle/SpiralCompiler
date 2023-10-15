using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Symbols;

public abstract class Symbol
{
    public virtual string Name => string.Empty;
    public virtual IEnumerable<Symbol> Members => Enumerable.Empty<Symbol>();
    public virtual Symbol? ContainingSymbol => null;
    public virtual Compilation Compilation => ContainingSymbol?.Compilation ?? throw new InvalidOperationException();

    public override string ToString() => Name;
}

public abstract class ModuleSymbol : Symbol
{

}

public abstract class TypeSymbol : Symbol
{
    public IEnumerable<TypeSymbol> BaseTypes => ImmediateBaseTypes.SelectMany(b => b.BaseTypes).Append(this);
    public virtual IEnumerable<TypeSymbol> ImmediateBaseTypes => Enumerable.Empty<TypeSymbol>();
}

public abstract class LocalVariableSymbol : Symbol
{
    public abstract TypeSymbol Type { get; }
}

public abstract class GlobalVariableSymbol : Symbol
{
    public abstract TypeSymbol Type { get; }
}

public abstract class ParameterSymbol : LocalVariableSymbol
{

}

public abstract class FunctionSymbol : Symbol
{
    public static string GetBinaryOperatorName(TokenType op) => op switch
    {
        TokenType.Plus => "binary operator+",
        TokenType.Minus => "binary operator-",
        TokenType.LessThan => "binary operator<",
        _ => throw new ArgumentOutOfRangeException(nameof(op)),
    };

    public static string GetPrefixUnaryOperatorName(TokenType op) => op switch
    {
        TokenType.Increment => "pre operator++",
        _ => throw new ArgumentOutOfRangeException(nameof(op)),
    };

    public abstract ImmutableArray<ParameterSymbol> Parameters { get; }
    public abstract TypeSymbol ReturnType { get; }
}

public sealed class OverloadSymbol : Symbol
{
    public override string Name => Functions[0].Name;
    public ImmutableArray<FunctionSymbol> Functions { get; }

    public OverloadSymbol(ImmutableArray<FunctionSymbol> functions)
    {
        Functions = functions;
    }
}
