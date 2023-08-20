using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.Binding;
public sealed class ModuleBinder : Binder
{
    public override Compilation Compilation => module.Compilation;

    public override Binder Parent { get; }

    public override IEnumerable<Symbol> DeclaredSymbols => module.Members;

    private readonly ModuleSymbol module;

    public ModuleBinder(Binder parent, ModuleSymbol module)
    {
        Parent = parent;
        this.module = module;
    }
}
