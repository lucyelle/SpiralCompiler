using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.Binding;

public sealed class RootBinder : Binder
{
    public override Binder? Parent => null;

    public override IEnumerable<Symbol> DeclaredSymbols => Enumerable.Empty<Symbol>();
}
