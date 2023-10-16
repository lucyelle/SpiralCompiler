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
        IfStatementSyntax fi => BindIfStatement(fi),
        WhileStatementSyntax wh => BindWhileStatement(wh),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public BoundExpression BindExpression(ExpressionSyntax syntax) => syntax switch
    {
        NameExpressionSyntax name => BindNameExpression(name),
        LiteralExpressionSyntax lit => BindLiteralExpression(lit),
        PrefixUnaryExpressionSyntax pfx => BindPrefixUnaryExpression(pfx),
        BinaryExpressionSyntax bin => BindBinaryExpression(bin),
        CallExpressionSyntax call => BindCallExpression(call),
        NewExpressionSyntax nw => BindNewExpression(nw),
        MemberExpressionSyntax mem => BindMemberExpression(mem),
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
        var symbol = DeclaredSymbols
            .OfType<SourceLocalVariableSymbol>()
            .Single(s => s.Syntax == decl);
        var declaredType = decl.Type is null
            ? null
            : BindType(decl.Type.Type);

        if (decl.Value is null)
        {
            if (declaredType is null)
            {
                throw new InvalidOperationException("type or value must be specified");
            }
            symbol.SetType(declaredType);
            return new BoundNopStatement(null);
        }
        else
        {
            var left = new BoundLocalVariableExpression(decl.Name, symbol);
            var right = BindExpression(decl.Value.Value);
            var variableType = declaredType ?? right.Type;
            symbol.SetType(variableType);
            if (declaredType is not null)
            {
                TypeSystem.Assignable(declaredType, right.Type);
            }
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

    private BoundStatement BindIfStatement(IfStatementSyntax fi)
    {
        var condition = BindExpression(fi.Condition);
        TypeSystem.Condition(condition.Type);
        var then = BindStatement(fi.Then);
        var els = fi.Else is null ? null : BindStatement(fi.Else.Body);
        return new BoundIfStatement(fi, condition, then, els);
    }

    private BoundStatement BindWhileStatement(WhileStatementSyntax wh)
    {
        var condition = BindExpression(wh.Condition);
        TypeSystem.Condition(condition.Type);
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
        else if (symbol is FieldSymbol field)
        {
            return new BoundFieldExpression(name, null, field);
        }
        else
        {
            // TODO: error handling
            throw new NotImplementedException();
        }
    }

    private BoundExpression BindPrefixUnaryExpression(PrefixUnaryExpressionSyntax pfx)
    {
        var subexpr = BindExpression(pfx.Right);

        var operatorName = FunctionSymbol.GetPrefixUnaryOperatorName(pfx.Op.Type);
        var overloadSet = (OverloadSymbol)LookUp(operatorName)!;
        var opSymbol = TypeSystem.ResolveOverload(overloadSet, ImmutableArray.Create(subexpr.Type));

        return new BoundCallExpression(pfx, opSymbol, ImmutableArray.Create(subexpr));
    }

    private BoundExpression BindBinaryExpression(BinaryExpressionSyntax bin)
    {
        var left = BindExpression(bin.Left);
        var right = BindExpression(bin.Right);

        if (bin.Op.Type == TokenType.Assign)
        {
            TypeSystem.Assignable(left.Type, right.Type);
            return new BoundAssignmentExpression(bin, left, right);
        }
        else
        {
            var operatorName = FunctionSymbol.GetBinaryOperatorName(bin.Op.Type);
            // NOTE: it is safe to cast here, because the name of an operator is very special
            // We can guarantee that it will always be an overload symbol and nothing else
            var overloadSet = (OverloadSymbol)LookUp(operatorName)!;
            var opSymbol = TypeSystem.ResolveOverload(overloadSet, ImmutableArray.Create(left.Type, right.Type));
            return new BoundCallExpression(bin, opSymbol, ImmutableArray.Create(left, right));
        }
    }

    private BoundExpression BindCallExpression(CallExpressionSyntax call)
    {
        var func = BindExpression(call.Function);
        var args = call.Args.Values
            .Select(BindExpression)
            .ToImmutableArray();
        if (func is BoundOverloadExpression overload)
        {
            var chosen = TypeSystem.ResolveOverload(overload.Overload, args.Select(a => a.Type).ToImmutableArray());
            return new BoundCallExpression(call, chosen, args);
        }
        else
        {
            // TODO
            throw new NotImplementedException();
        }
    }

    private BoundExpression BindNewExpression(NewExpressionSyntax nw)
    {
        var ty = this.BindType(nw.Type);
        if (ty is not ClassSymbol classType)
        {
            throw new InvalidOperationException("can only instantiate classes");
        }

        var ctorOverloads = classType.Constructors;
        var args = nw.Args.Values
            .Select(BindExpression)
            .ToImmutableArray();
        var ctor = TypeSystem.ResolveOverload(ctorOverloads, args.Select(a => a.Type).ToImmutableArray());

        return new BoundCallExpression(nw, ctor, args);
    }

    private BoundExpression BindMemberExpression(MemberExpressionSyntax mem)
    {
        var left = BindExpression(mem.Left);
        var member = left.Type.Members
            .Where(m => m.Name == mem.Member.Text)
            .ToImmutableArray();

        if (member.Length == 0)
        {
            throw new InvalidOperationException($"no member {mem.Member.Text} in {left.Type}");
        }

        if (member.Length == 1 && member[0] is FieldSymbol field)
        {
            return new BoundFieldExpression(mem, left, field);
        }

        throw new NotImplementedException();
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
