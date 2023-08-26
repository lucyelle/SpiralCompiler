using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.BoundTree;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Binding;

public abstract class Binder
{
    public virtual Compilation Compilation => Parent?.Compilation ?? throw new InvalidOperationException();

    public abstract Binder? Parent { get; }

    public abstract IEnumerable<Symbol> DeclaredSymbols { get; }

    private Binder GetBinder(SyntaxNode node) => Compilation.BinderCache.GetBinder(node);

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
        BlockStatementSyntax block => BindBlockStatement(block),
        ReturnStatementSyntax ret => BindReturnStatement(ret),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public BoundExpression BindExpression(ExpressionSyntax syntax) => syntax switch
    {
        NameExpressionSyntax name => BindNameExpression(name),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public TypeSymbol BindType(TypeSyntax syntax) => syntax switch
    {
        NameTypeSyntax name => BindNameType(name),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    private BoundStatement BindBlockStatement(BlockStatementSyntax block)
    {
        var binder = GetBinder(block);
        var statements = block.Statements.Select(s => binder.BindStatement(s)).ToImmutableArray();
        return new BoundBlockStatement(block, statements);
    }

    private BoundStatement BindReturnStatement(ReturnStatementSyntax ret)
    {
        var value = ret.Value is null ? null : BindExpression(ret.Value);
        return new BoundReturnStatement(ret, value);
    }

    private BoundExpression BindNameExpression(NameExpressionSyntax name)
    {
        var symbol = LookUp(name.Name.Text);

        if (symbol is LocalVariableSymbol local)
        {
            return new BoundLocalVariableExpression(name, local);
        }
        else
        {
            // TODO: error handling
            throw new NotImplementedException();
        }
    }

    private TypeSymbol BindNameType(NameTypeSyntax name)
    {
        var symbol = LookUp(name.Name.Text);

        if (symbol is TypeSymbol type)
        {
            return type;
        }
        else
        {
            // TODO: error handling
            throw new NotImplementedException();
        }
    }

}
