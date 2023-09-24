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
        VariableDeclarationSyntax decl => BindVariableDeclaration(decl),
        BlockStatementSyntax block => BindBlockStatement(block),
        ReturnStatementSyntax ret => BindReturnStatement(ret),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public BoundExpression BindExpression(ExpressionSyntax syntax) => syntax switch
    {
        NameExpressionSyntax name => BindNameExpression(name),
        LiteralExpressionSyntax lit => BindLiteralExpression(lit),
        BinaryExpressionSyntax bin => BindBinaryExpression(bin),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public TypeSymbol BindType(TypeSyntax syntax) => syntax switch
    {
        NameTypeSyntax name => BindNameType(name),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax decl)
    {
        if (decl.Value is null)
        {
            return new BoundNopStatement(null);
        }
        else
        {
            var symbol = this.DeclaredSymbols
                .OfType<SourceLocalVariableSymbol>()
                .Single(s => s.Syntax == decl);
            var left = new BoundLocalVariableExpression(decl.Name, symbol);
            var right = BindExpression(decl.Value.Value);
            return new BoundExpressionStatement(decl, new BoundAssignmentExpression(decl, left, right));
        }
    }

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

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax lit) => lit.Value.Type switch
    {
        TokenType.Integer => new BoundLiteralExpression(lit, int.Parse(lit.Value.Text)),
        _ => throw new ArgumentOutOfRangeException(nameof(lit)),
    };

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

    private BoundExpression BindBinaryExpression(BinaryExpressionSyntax bin)
    {
        var left = BindExpression(bin.Left);
        var right = BindExpression(bin.Right);

        var operatorName = FunctionSymbol.GetOperatorName(bin.Op.Type);
        // NOTE: it is safe to cast here, because the name of an operator is very special
        // We can guarantee that it will always be a function symbol and nothing else
        var opSymbol = (FunctionSymbol)LookUp(operatorName)!;

        // TODO: Check overloading
        return new BoundCallExpression(bin, opSymbol, ImmutableArray.Create(left, right));
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
