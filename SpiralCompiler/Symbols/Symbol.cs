using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.BoundTree;
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

public abstract class InterfaceSymbol : TypeSymbol
{
}

public abstract class ClassSymbol : TypeSymbol
{
    public abstract OverloadSymbol Constructors { get; }
    public abstract IEnumerable<FieldSymbol> Fields { get; }
}

public abstract class FieldSymbol : Symbol
{
    public abstract TypeSymbol Type { get; }
}

public abstract class LocalVariableSymbol : Symbol
{
    public abstract TypeSymbol Type { get; }
}

public abstract class GlobalVariableSymbol : Symbol
{
    public abstract TypeSymbol Type { get; }
    public abstract BoundExpression? InitialValue { get; }
}

public abstract class ParameterSymbol : LocalVariableSymbol
{

}

public abstract class FunctionSymbol : Symbol
{
    public static string GetBinaryOperatorName(TokenType op) => op switch
    {
        TokenType.Plus => "operator+",
        TokenType.Multiply => "operator*",
        TokenType.Minus => "operator-",
        TokenType.Divide => "operator/",
        TokenType.Modulo => "operator%",
        TokenType.LessThan => "operator<",
        TokenType.GreaterThan => "operator>",
        TokenType.LessEquals => "operator<=",
        TokenType.Equals => "operator==",
        TokenType.GreaterEquals => "operator>=",
        _ => throw new ArgumentOutOfRangeException(nameof(op)),
    };

    public static string GetPrefixUnaryOperatorName(TokenType op) => op switch
    {
        TokenType.Minus => "operator-",
        TokenType.Not => "operator not",
        _ => throw new ArgumentOutOfRangeException(nameof(op)),
    };

    public abstract ImmutableArray<ParameterSymbol> Parameters { get; }
    public abstract TypeSymbol ReturnType { get; }
    public virtual bool IsConstructor => false;
    public virtual bool IsInstance => IsConstructor;
    public InterfaceSymbol? OriginatingInterface => BaseDeclaration.ContainingSymbol as InterfaceSymbol;
    public bool IsVirtual => OriginatingInterface is not null;
    public virtual FunctionSymbol BaseDeclaration => this;

    public bool SignatureEquals(FunctionSymbol other)
    {
        if (Name != other.Name) return false;
        if (Parameters.Length != other.Parameters.Length) return false;
        if (ReturnType != other.ReturnType) return false;

        for (var i = 0; i < Parameters.Length; i++)
        {
            if (Parameters[i].Type != other.Parameters[i].Type) return false;
        }

        return true;
    }
}

public sealed class OverloadSymbol : Symbol
{
    public override string Name { get; }
    public ImmutableArray<FunctionSymbol> Functions { get; }

    public OverloadSymbol(string name, ImmutableArray<FunctionSymbol> functions)
    {
        Name = name;
        Functions = functions;
    }
}
