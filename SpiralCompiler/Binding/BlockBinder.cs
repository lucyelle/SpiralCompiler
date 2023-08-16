using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Binding;

public sealed class BlockBinder : Binder
{
    public override Binder Parent { get; }

    public override IEnumerable<Symbol> DeclaredSymbols => throw new NotImplementedException();

    private readonly BlockStatementSyntax blockStatement;

    public BlockBinder(Binder parent, BlockStatementSyntax blockStatement)
    {
        Parent = parent;
        this.blockStatement = blockStatement;
    }
}
