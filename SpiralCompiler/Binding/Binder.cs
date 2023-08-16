using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.BoundTree;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Binding;

public abstract class Binder
{
    public abstract Binder? Parent { get; }
    public abstract IEnumerable<Symbol> DeclaredSymbols { get; }

    public Symbol? LookUp(string name)
    {
        foreach (var symbol in DeclaredSymbols)
        {
            if (symbol.Name == name) return symbol;
        }
        return Parent?.LookUp(name);
    }

    public BoundStatement BindStatement(StatementSyntax syntax) => syntax switch
    {
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public BoundExpression BindExpression(ExpressionSyntax syntax) => syntax switch
    {
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };
}
