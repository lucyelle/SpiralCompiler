using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

public sealed record class BoundExpressionStatement(SyntaxNode? Syntax, BoundExpression Expression) : BoundStatement(Syntax);

public sealed record class BoundIfStatement(
    SyntaxNode? Syntax,
    BoundExpression Condition,
    BoundStatement Then,
    BoundStatement? Else) : BoundStatement(Syntax);

public sealed record class BoundWhileStatement(
    SyntaxNode? Syntax,
    BoundExpression Condition,
    BoundStatement Body) : BoundStatement(Syntax);

public sealed record class BoundBlockStatement(
    SyntaxNode? Syntax,
    ImmutableArray<BoundStatement> Statements) : BoundStatement(Syntax);

public sealed record class BoundReturnStatement(
    SyntaxNode? Syntax,
    BoundExpression? Expression) : BoundStatement(Syntax);

public sealed record class BoundCallExpression(
    SyntaxNode? Syntax,
    FunctionSymbol Function,
    ImmutableArray<BoundExpression> Args) : BoundExpression(Syntax)
{
    public override TypeSymbol Type => Function.ReturnType;
}

public sealed record class BoundLocalVariableExpression(
    SyntaxNode? Syntax,
    LocalVariableSymbol Variable) : BoundExpression(Syntax)
{
    public override TypeSymbol Type => Variable.Type;
}
