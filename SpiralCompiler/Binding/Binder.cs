using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        return new OverloadSymbol(name, overloads.ToImmutable());
    }

    public BoundStatement BindStatement(StatementSyntax syntax, List<ErrorMessage> errors) => syntax switch
    {
        UnexpectedDeclarationSyntax unexpected => new BoundBlockStatement(unexpected, ImmutableArray<BoundStatement>.Empty),
        ExpressionStatementSyntax expr => BindExpressionStatement(expr, errors),
        VariableDeclarationSyntax decl => BindVariableDeclaration(decl, errors),
        BlockStatementSyntax block => BindBlockStatement(block, errors),
        ReturnStatementSyntax ret => BindReturnStatement(ret, errors),
        IfStatementSyntax fi => BindIfStatement(fi, errors),
        WhileStatementSyntax wh => BindWhileStatement(wh, errors),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public BoundExpression BindExpression(ExpressionSyntax syntax, List<ErrorMessage> errors) => syntax switch
    {
        UnexpectedExpressionSyntax unexpected => new BoundErrorExpression(unexpected),
        GroupExpressionSyntax grp => BindExpression(grp.Subexpression, errors),
        NameExpressionSyntax name => BindNameExpression(name, errors),
        LiteralExpressionSyntax lit => BindLiteralExpression(lit, errors),
        PrefixUnaryExpressionSyntax pfx => BindPrefixUnaryExpression(pfx, errors),
        BinaryExpressionSyntax bin => BindBinaryExpression(bin, errors),
        CallExpressionSyntax call => BindCallExpression(call, errors),
        IndexExpressionSyntax index => BindIndexExpression(index, errors),
        NewExpressionSyntax nw => BindNewExpression(nw, errors),
        MemberExpressionSyntax mem => BindMemberExpression(mem, errors),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    public TypeSymbol BindType(TypeSyntax syntax, List<ErrorMessage> errors) => syntax switch
    {
        UnexpectedTypeSyntax unexpected => BuiltInTypeSymbol.Error,
        NameTypeSyntax name => BindNameType(name, errors),
        _ => throw new ArgumentOutOfRangeException(nameof(syntax))
    };

    private BoundStatement BindExpressionStatement(ExpressionStatementSyntax expr, List<ErrorMessage> errors)
    {
        var subexpr = BindExpression(expr.Expression, errors);
        return new BoundExpressionStatement(expr, subexpr);
    }

    private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax decl, List<ErrorMessage> errors)
    {
        var symbol = DeclaredSymbols
            .OfType<SourceLocalVariableSymbol>()
            .Single(s => s.Syntax == decl);

        var declaredType = decl.Type is null
            ? null
            : BindType(decl.Type.Type, errors);

        if (decl.Value is null)
        {
            return new BoundNopStatement(null);
        }
        else
        {
            var left = new BoundLocalVariableExpression(decl.Name, symbol);
            var right = BindExpression(decl.Value.Value, errors);
            return new BoundExpressionStatement(decl, new BoundAssignmentExpression(decl, left, right));
        }
    }

    private BoundStatement BindBlockStatement(BlockStatementSyntax block, List<ErrorMessage> errors)
    {
        var binder = GetBinder(block);
        var statements = block.Statements.Select(s => binder.BindStatement(s, errors)).ToImmutableArray();
        return new BoundBlockStatement(block, statements);
    }

    private BoundStatement BindReturnStatement(ReturnStatementSyntax ret, List<ErrorMessage> errors)
    {
        // TODO: Check return type compatibility
        var value = ret.Value is null ? null : BindExpression(ret.Value, errors);
        return new BoundReturnStatement(ret, value);
    }

    private BoundStatement BindIfStatement(IfStatementSyntax fi, List<ErrorMessage> errors)
    {
        var condition = BindExpression(fi.Condition, errors);
        TypeSystem.Condition(fi.Condition, condition.Type, errors);
        var then = BindStatement(fi.Then, errors);
        var els = fi.Else is null ? null : BindStatement(fi.Else.Body, errors);
        return new BoundIfStatement(fi, condition, then, els);
    }

    private BoundStatement BindWhileStatement(WhileStatementSyntax wh, List<ErrorMessage> errors)
    {
        var condition = BindExpression(wh.Condition, errors);
        TypeSystem.Condition(wh.Condition, condition.Type, errors);
        var body = BindStatement(wh.Body, errors);
        return new BoundWhileStatement(wh, condition, body);
    }

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax lit, List<ErrorMessage> errors) => lit.Value.Type switch
    {
        TokenType.Integer => new BoundLiteralExpression(lit, int.Parse(lit.Value.Text)),
        TokenType.String => new BoundLiteralExpression(lit, UnescapeString(lit.Value)),
        TokenType.Boolean => new BoundLiteralExpression(lit, lit.Value.Text == "true"),
        _ => throw new ArgumentOutOfRangeException(nameof(lit)),
    };

    private static string UnescapeString(Token token) => token.Text[1..^1];

    private BoundExpression BindNameExpression(NameExpressionSyntax name, List<ErrorMessage> errors)
    {
        var symbol = LookUp(name.Name.Text);

        if (symbol is null)
        {
            errors.Add(new ErrorMessage($"unknown symbol {name.Name.Text}", name));
            return new BoundErrorExpression(name);
        }
        else if (symbol is GlobalVariableSymbol global)
        {
            return new BoundGlobalVariableExpression(name, global);
        }
        else if (symbol is LocalVariableSymbol local)
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
            errors.Add(new ErrorMessage($"symbol {name.Name.Text} is not a value symbol", name));
            return new BoundErrorExpression(name);
        }
    }

    private BoundExpression BindPrefixUnaryExpression(PrefixUnaryExpressionSyntax pfx, List<ErrorMessage> errors)
    {
        var subexpr = BindExpression(pfx.Right, errors);

        var operatorName = FunctionSymbol.GetPrefixUnaryOperatorName(pfx.Op.Type);
        var overloadSet = (OverloadSymbol)LookUp(operatorName)!;
        var opSymbol = TypeSystem.ResolveOverload(pfx, overloadSet, ImmutableArray.Create(subexpr.Type), errors);

        return new BoundCallExpression(pfx, opSymbol, ImmutableArray.Create(subexpr));
    }

    private BoundExpression BindBinaryExpression(BinaryExpressionSyntax bin, List<ErrorMessage> errors)
    {
        var left = BindExpression(bin.Left, errors);
        var right = BindExpression(bin.Right, errors);

        if (bin.Op.Type == TokenType.Assign)
        {
            TypeSystem.Assignable(bin, left.Type, right.Type, errors);
            return new BoundAssignmentExpression(bin, left, right);
        }
        else if (bin.Op.Type == TokenType.And)
        {
            TypeSystem.Condition(bin.Left, left.Type, errors);
            TypeSystem.Condition(bin.Right, right.Type, errors);
            return new BoundAndExpression(bin, left, right);
        }
        else if (bin.Op.Type == TokenType.Or)
        {
            TypeSystem.Condition(bin.Left, left.Type, errors);
            TypeSystem.Condition(bin.Right, right.Type, errors);
            return new BoundOrExpression(bin, left, right);
        }
        else
        {
            var operatorName = FunctionSymbol.GetBinaryOperatorName(bin.Op.Type);
            // NOTE: it is safe to cast here, because the name of an operator is very special
            // We can guarantee that it will always be an overload symbol and nothing else
            var overloadSet = (OverloadSymbol)LookUp(operatorName)!;
            var opSymbol = TypeSystem.ResolveOverload(bin, overloadSet, ImmutableArray.Create(left.Type, right.Type), errors);
            return new BoundCallExpression(bin, opSymbol, ImmutableArray.Create(left, right));
        }
    }

    private BoundExpression BindCallExpression(CallExpressionSyntax call, List<ErrorMessage> errors)
    {
        var func = BindExpression(call.Function, errors);
        var args = call.Args.Values
            .Select(e => BindExpression(e, errors))
            .ToImmutableArray();
        if (func is BoundOverloadExpression overload)
        {
            var chosen = TypeSystem.ResolveOverload(call, overload.Overload, args.Select(a => a.Type).ToImmutableArray(), errors);
            if (chosen.IsInstance)
            {
                return new BoundMemberCallExpression(call, chosen, null, args);
            }
            else
            {
                return new BoundCallExpression(call, chosen, args);
            }
        }
        else if (func is BoundFunctionGroupExpression group)
        {
            var chosen = TypeSystem.ResolveOverload(call, group.Overload, args.Select(a => a.Type).ToImmutableArray(), errors);
            return new BoundMemberCallExpression(call, chosen, group.Receiver, args);
        }
        else
        {
            errors.Add(new ErrorMessage("the expression can not be called", call));
            return new BoundErrorExpression(call);
        }
    }

    private BoundExpression BindIndexExpression(IndexExpressionSyntax index, List<ErrorMessage> errors)
    {
        // NOTE: For now we only implement string indexing
        var array = BindExpression(index.Array, errors);
        var args = index.Args.Values
            .Select(e => BindExpression(e, errors))
            .ToImmutableArray();

        if (array.Type != BuiltInTypeSymbol.String)
        {
            errors.Add(new ErrorMessage("can only index strings", index));
            return new BoundErrorExpression(index);
        }
        if (args.Length != 1 || (args[0].Type != BuiltInTypeSymbol.Int && args[0].Type != BuiltInTypeSymbol.Error))
        {
            errors.Add(new ErrorMessage("index must be an integer", index.Args));
            return new BoundErrorExpression(index);
        }

        return new BoundElementAtExpression(index, array, args[0], BuiltInTypeSymbol.String);
    }

    private BoundExpression BindNewExpression(NewExpressionSyntax nw, List<ErrorMessage> errors)
    {
        var ty = this.BindType(nw.Type, errors);
        if (ty is not ClassSymbol classType)
        {
            errors.Add(new ErrorMessage("can only instantiate classes", nw));
            return new BoundErrorExpression(nw);
        }

        var ctorOverloads = classType.Constructors;
        var args = nw.Args.Values
            .Select(e => BindExpression(e, errors))
            .ToImmutableArray();
        var ctor = TypeSystem.ResolveOverload(nw, ctorOverloads, args.Select(a => a.Type).ToImmutableArray(), errors);

        return new BoundCallExpression(nw, ctor, args);
    }

    private BoundExpression BindMemberExpression(MemberExpressionSyntax mem, List<ErrorMessage> errors)
    {
        var left = BindExpression(mem.Left, errors);
        var member = left.Type.Members
            .Where(m => m.Name == mem.Member.Text)
            .ToImmutableArray();

        if (member.Length == 0)
        {
            errors.Add(new($"no member '{mem.Member.Text}' in {left.Type}", mem));
            return new BoundErrorExpression(mem);
        }

        if (member.Length == 1 && member[0] is FieldSymbol field)
        {
            return new BoundFieldExpression(mem, left, field);
        }

        if (member.All(m => m is FunctionSymbol))
        {
            var functions = member.Cast<FunctionSymbol>().ToImmutableArray();
            return new BoundFunctionGroupExpression(mem, left, new(member.First().Name, functions));
        }

        throw new NotImplementedException("unknown member set");
    }

    private TypeSymbol BindNameType(NameTypeSyntax name, List<ErrorMessage> errors)
    {
        var symbol = LookUp(name.Name.Text);

        if (symbol is null)
        {
            errors.Add(new ErrorMessage($"unknown symbol {name.Name.Text}", name));
            return BuiltInTypeSymbol.Error;
        }
        if (symbol is TypeSymbol type)
        {
            return type;
        }
        else
        {
            errors.Add(new ErrorMessage($"symbol {name.Name.Text} is not a type", name));
            return BuiltInTypeSymbol.Error;
        }
    }
}
