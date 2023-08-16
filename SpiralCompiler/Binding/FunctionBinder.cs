using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.Binding;

public sealed class FunctionBinder : Binder
{
    public override Binder Parent { get; }

    public override IEnumerable<Symbol> DeclaredSymbols => function.Parameters;

    private readonly FunctionSymbol function;

    public FunctionBinder(Binder parent, FunctionSymbol function)
    {
        Parent = parent;
        this.function = function;
    }
}
