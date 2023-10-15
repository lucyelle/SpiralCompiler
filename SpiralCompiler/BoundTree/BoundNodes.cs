using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.BoundTree;

public abstract record class BoundNode(SyntaxNode? Syntax);

public abstract record class BoundExpression(SyntaxNode? Syntax) : BoundNode(Syntax)
{
    public abstract TypeSymbol Type { get; }
}

public abstract record class BoundStatement(SyntaxNode? Syntax) : BoundNode(Syntax);

public sealed record class BoundExpressionStatement(
    SyntaxNode? Syntax,
    BoundExpression Expression) : BoundStatement(Syntax)
{
    public override string ToString() => $"ExpressionStatement({Expression})";
}

public sealed record class BoundNopStatement(SyntaxNode? Syntax) : BoundStatement(Syntax)
{
    public override string ToString() => "NopStatement";

}

public sealed record class BoundIfStatement(
    SyntaxNode? Syntax,
    BoundExpression Condition,
    BoundStatement Then,
    BoundStatement? Else) : BoundStatement(Syntax)
{
    public override string ToString() => $"IfStatement({Condition}, {Then}, {Else})";
}

public sealed record class BoundWhileStatement(
    SyntaxNode? Syntax,
    BoundExpression Condition,
    BoundStatement Body) : BoundStatement(Syntax)
{
    public override string ToString() => $"WhileStatement({Condition}, {Body})";
}

public sealed record class BoundBlockStatement(
    SyntaxNode? Syntax,
    ImmutableArray<BoundStatement> Statements) : BoundStatement(Syntax)
{
    public override string ToString() => $"BlockStatement({string.Join(", ", Statements)})";
}

public sealed record class BoundReturnStatement(
    SyntaxNode? Syntax,
    BoundExpression? Expression) : BoundStatement(Syntax)
{
    public override string ToString() => $"ReturnStatement({Expression})";
}

public sealed record class BoundCallExpression(
    SyntaxNode? Syntax,
    FunctionSymbol Function,
    ImmutableArray<BoundExpression> Args) : BoundExpression(Syntax)
{
    public override TypeSymbol Type => Function.ReturnType;

    public override string ToString() => $"CallExpression({Function}, [{string.Join(", ", Args)}])";

}

public sealed record class BoundLocalVariableExpression(
    SyntaxNode? Syntax,
    LocalVariableSymbol Variable) : BoundExpression(Syntax)
{
    public override TypeSymbol Type => Variable.Type;
    public override string ToString() => $"LocalVariableExpression({Variable.Name})";
}

public sealed record class BoundLiteralExpression(
    SyntaxNode? Syntax,
    object? Value) : BoundExpression(Syntax)
{
    public override TypeSymbol Type => Value switch
    {
        int => BuiltInTypeSymbol.Int,
        _ => throw new InvalidOperationException(),
    };
    public override string ToString() => $"LiteralExpression({Value})";
}

public sealed record class BoundAssignmentExpression(
    SyntaxNode? Syntax,
    BoundExpression Target,
    BoundExpression Source) : BoundExpression(Syntax)
{
    public override TypeSymbol Type => Target.Type;
    public override string ToString() => $"AssignmentExpression({Target}, {Source})";
}

public sealed record class BoundOverloadExpression(
    SyntaxNode? Syntax,
    OverloadSymbol Overload) : BoundExpression(Syntax)
{
    public override TypeSymbol Type => throw new InvalidOperationException();
    public override string ToString() => $"OverloadExpression({Overload})";
}

public sealed record class BoundFieldExpression(
    SyntaxNode? Syntax,
    BoundExpression Receiver,
    FieldSymbol Field) : BoundExpression(Syntax)
{
    public override TypeSymbol Type => Field.Type;
    public override string ToString() => $"FieldExpression({Receiver}, {Field})";
}
