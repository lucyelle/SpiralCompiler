using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public abstract ImmutableArray<ParameterSymbol> Parameters { get; }
    public abstract TypeSymbol ReturnType { get; }
}
