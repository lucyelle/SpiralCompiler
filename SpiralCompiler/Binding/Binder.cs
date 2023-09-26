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
        var overloads = ImmutableArray.CreateBuilder<FunctionSymbol>();
        foreach (var symbol in DeclaredSymbols)
        {
            if (symbol.Name == name)
            {
                if (symbol is not FunctionSymbol f) return symbol;
                overloads.Add(f);
            }
        }
        if (overloads.Count == 0) return Parent?.LookUp(name);
        return new OverloadSymbol(overloads.ToImmutable());
    }

    public BoundStatement BindStatement(StatementSyntax syntax) => syntax switch
    {
        ExpressionStatementSyntax expr => BindExpressionStatement(expr),
        VariableDeclarationSyntax decl => BindVariableDeclaration(decl),
        BlockStatementSyntax block => BindBlockStatement(block),
        ReturnStatementSyntax ret => BindReturnStatement(ret),
        WhileStatementSyntax wh => BindWhileStatement(wh),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public BoundExpression BindExpression(ExpressionSyntax syntax) => syntax switch
    {
        NameExpressionSyntax name => BindNameExpression(name),
        LiteralExpressionSyntax lit => BindLiteralExpression(lit),
        BinaryExpressionSyntax bin => BindBinaryExpression(bin),
        CallExpressionSyntax call => BindCallExpression(call),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public TypeSymbol BindType(TypeSyntax syntax) => syntax switch
    {
        NameTypeSyntax name => BindNameType(name),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    private BoundStatement BindExpressionStatement(ExpressionStatementSyntax expr)
    {
        var subexpr = BindExpression(expr.Expression);
        return new BoundExpressionStatement(expr, subexpr);
    }

    private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax decl)
    {
        if (decl.Value is null)
        {
            return new BoundNopStatement(null);
        }
        else
        {
            // TODO: Check assignability
            var symbol = this.DeclaredSymbols
                .OfType<SourceLocalVariableSymbol>()
                .Single(s => s.Syntax == decl);
            var declaredType = decl.Type is null
                ? null
                : BindType(decl.Type.Type);
            var left = new BoundLocalVariableExpression(decl.Name, symbol);
            var right = BindExpression(decl.Value.Value);
            symbol.SetType(declaredType ?? right.Type);
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
        // TODO: Check return type compatibility
        var value = ret.Value is null ? null : BindExpression(ret.Value);
        return new BoundReturnStatement(ret, value);
    }

    private BoundStatement BindWhileStatement(WhileStatementSyntax wh)
    {
        // TODO: Check if condition type is bool
        var condition = BindExpression(wh.Condition);
        var body = BindStatement(wh.Body);
        return new BoundWhileStatement(wh, condition, body);
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
        else if (symbol is OverloadSymbol overload)
        {
            return new BoundOverloadExpression(name, overload);
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
        // We can guarantee that it will always be an overload symbol and nothing else
        var overloadSet = (OverloadSymbol)LookUp(operatorName)!;
        var opSymbol = ResolveOverload(overloadSet, ImmutableArray.Create(left.Type, right.Type));

        // TODO: Check overloading
        return new BoundCallExpression(bin, opSymbol, ImmutableArray.Create(left, right));
    }

    private BoundExpression BindCallExpression(CallExpressionSyntax call)
    {
        var func = BindExpression(call.Function);
        var args = call.Args.Values
            .Select(BindExpression)
            .ToImmutableArray();
        if (func is BoundOverloadExpression overload)
        {
            var chosen = ResolveOverload(overload.Overload, args.Select(a => a.Type).ToImmutableArray());
            return new BoundCallExpression(call, chosen, args);
        }
        else
        {
            // TODO
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

    private static FunctionSymbol ResolveOverload(OverloadSymbol set, ImmutableArray<TypeSymbol> parameterTypes)
    {
        foreach (var func in set.Functions)
        {
            if (MatchesOverload(func, parameterTypes)) return func;
        }
        // TODO
        throw new NotImplementedException();
    }

    private static bool MatchesOverload(FunctionSymbol overload, ImmutableArray<TypeSymbol> parameterTypes)
    {
        return overload.Parameters.Select(p => p.Type).SequenceEqual(parameterTypes);
    }
}
