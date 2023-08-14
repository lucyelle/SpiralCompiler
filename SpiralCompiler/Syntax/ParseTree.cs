using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.Syntax;

public abstract record class SyntaxNode;

public sealed record class SeparatedSyntaxList<T>(ImmutableArray<SyntaxNode> Nodes) : SyntaxNode
    where T : SyntaxNode
{
    public IEnumerable<T> Values
    {
        get
        {
            for (var i = 0; i < Nodes.Length; i += 2)
            {
                yield return (T)Nodes[i];
            }
        }
    }

    public override string ToString() => $"[{string.Join(", ", Nodes)}]";
}

public sealed record class SyntaxList<T>(ImmutableArray<T> Nodes) : SyntaxNode
    where T : SyntaxNode
{
    public static implicit operator SyntaxList<T>(ImmutableArray<T> nodes) => new SyntaxList<T>(nodes);
    public override string ToString() => $"[{string.Join(", ", Nodes)}]";
}

public abstract record class DeclarationSyntax : StatementSyntax;

public abstract record class StatementSyntax : SyntaxNode;

public abstract record class ExpressionSyntax : SyntaxNode;

public abstract record class TypeSyntax : SyntaxNode;

public sealed record class ProgramSyntax(SyntaxList<DeclarationSyntax> Declarations) : SyntaxNode;

public sealed record class ExpressionStatementSyntax(ExpressionSyntax Expression, Token Semicolon) : StatementSyntax;

public sealed record class FunctionDeclarationSyntax(
    Token KeywordFunc,
    Token Name,
    Token ParenOpen,
    SeparatedSyntaxList<ParameterSyntax> Parameters,
    Token ParenClose,
    TypeSpecifierSyntax? ReturnType,
    BlockStatementSyntax Block
    ) : DeclarationSyntax;

public sealed record class VariableDeclarationSyntax(
    Token KeywordVar,
    Token Name,
    TypeSpecifierSyntax? Type,
    ValueSpecifierSyntax? Value,
    Token Semicolon
    ) : DeclarationSyntax;

public sealed record class IfStatementSyntax(
    Token KeywordIf,
    Token ParenOpen,
    ExpressionSyntax Condition,
    Token ParenClose,
    StatementSyntax Body,
    ElseSyntax? Else
    ) : StatementSyntax;

public sealed record class WhileStatementSyntax(
    Token KeywordWhile,
    Token ParenOpen,
    ExpressionSyntax Condition,
    Token ParenClose,
    StatementSyntax Body
    ) : StatementSyntax;

public sealed record class ReturnStatementSyntax(Token KeywordReturn, ExpressionSyntax? Value, Token Semicolon) : StatementSyntax;

public sealed record class ElseSyntax(
    Token KeywordElse,
    StatementSyntax Body) : SyntaxNode;

public sealed record class CallExpressionSyntax(
    ExpressionSyntax Function,
    Token ParenOpen,
    SeparatedSyntaxList<ExpressionSyntax> Args,
    Token ParenClose) : ExpressionSyntax;

public sealed record class GroupExpressionSyntax(
    Token ParenOpen,
    ExpressionSyntax Subexpression,
    Token ParenClose) : ExpressionSyntax;

public sealed record class LiteralExpressionSyntax(Token Value) : ExpressionSyntax;

public sealed record class BinaryExpressionSyntax(ExpressionSyntax Left, Token Op, ExpressionSyntax Right) : ExpressionSyntax;

public sealed record class PrefixUnaryExpressionSyntax(Token Op, ExpressionSyntax Right) : ExpressionSyntax;

public sealed record class PostfixUnaryExpressionSyntax(ExpressionSyntax Left, Token Op) : ExpressionSyntax;

public sealed record class NameExpressionSyntax(Token Name) : ExpressionSyntax;

public sealed record class ParameterSyntax(Token Name, Token Colon, TypeSyntax Type) : SyntaxNode;

public sealed record class TypeSpecifierSyntax(Token Colon, TypeSyntax Type) : SyntaxNode;

public sealed record class ValueSpecifierSyntax(Token Assign, ExpressionSyntax Value) : SyntaxNode;

public sealed record class BlockStatementSyntax(Token BraceOpen, SyntaxList<StatementSyntax> Statements, Token BraceClose) : StatementSyntax;

public sealed record class NameTypeSyntax(Token Name) : TypeSyntax;
