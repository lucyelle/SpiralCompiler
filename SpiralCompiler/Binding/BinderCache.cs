using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Binding;

public sealed class BinderCache
{
    private readonly Compilation compilation;

    private readonly Dictionary<SyntaxNode, Binder> binders = new();

    public BinderCache(Compilation compilation)
    {
        this.compilation = compilation;
    }

    public Binder GetBinder(SyntaxNode node)
    {
        node = GetScopeDefiningAncestor(node);

        if (binders.TryGetValue(node, out var binder))
        {
            return binder;
        }
        binder = ConstructBinder(node);
        binders.Add(node, binder);
        return binder;
    }

    private Binder ConstructBinder(SyntaxNode node)
    {
        switch (node)
        {
            case ProgramSyntax:
            {
                return new ModuleBinder(new RootBinder(), compilation.RootModule);
            }
            case FunctionDeclarationSyntax:
            {
                var parentBinder = GetBinder(GetParent(node));
                var symbol = parentBinder.DeclaredSymbols.OfType<SourceFunctionSymbol>().First(s => s.Syntax == node);
                return new FunctionBinder(parentBinder, symbol);
            }
            case ClassDeclarationSyntax:
            {
                var parentBinder = GetBinder(GetParent(node));
                var symbol = parentBinder.DeclaredSymbols.OfType<SourceClassSymbol>().First(s => s.Syntax == node);
                return new ClassBinder(parentBinder, symbol);
            }
            case BlockStatementSyntax block:
            {
                var parentBinder = GetBinder(GetParent(node));
                return new BlockBinder(parentBinder, block);
            }
            default: throw new ArgumentOutOfRangeException(nameof(node));
        }
    }

    private SyntaxNode GetScopeDefiningAncestor(SyntaxNode node)
    {
        while (!DefinesScope(node))
        {
            node = GetParent(node);
        }
        return node;
    }

    private SyntaxNode GetParent(SyntaxNode node) => compilation.Syntax.GetParent(node) ?? throw new InvalidOperationException();

    private static bool DefinesScope(SyntaxNode node) => node switch
    {
        ProgramSyntax => true,
        FunctionDeclarationSyntax => true,
        BlockStatementSyntax => true,
        ClassDeclarationSyntax => true,
        _ => false
    };
}
