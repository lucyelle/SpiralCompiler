using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Binding;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler;

public sealed class Compilation
{
    public ParseTree Syntax { get; }

    public ModuleSymbol RootModule => rootModule ??= new SourceModuleSymbol(Syntax.Root, null, this);

    private ModuleSymbol? rootModule;

    public BinderCache BinderCache { get; }

    public Compilation(ParseTree syntax)
    {
        Syntax = syntax;
        BinderCache = new(this);
    }
}
