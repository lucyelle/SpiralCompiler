using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.Syntax;

public sealed class ParseTree
{
    public ProgramSyntax Root { get; }

    private readonly Dictionary<SyntaxNode, SyntaxNode> paternity = new();

    public ParseTree(ProgramSyntax root)
    {
        Root = root;
    }

    public SyntaxNode? GetParent(SyntaxNode node)
    {
        if (paternity.Count == 0)
        {
            InsertChildrenOf(Root);
        }
        return paternity[node];
    }

    private void InsertChildrenOf(SyntaxNode node)
    {
        foreach (var child in node.Chilren)
        {
            paternity.Add(child, node);
            InsertChildrenOf(child);
        }
    }
}

public abstract record class SyntaxNode
{
    public abstract IEnumerable<SyntaxNode> Chilren { get; }
}

public sealed record class SeparatedSyntaxList<T>(ImmutableArray<SyntaxNode> Nodes) : SyntaxNode
    where T : SyntaxNode
{
    public override IEnumerable<SyntaxNode> Chilren => Nodes;

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

public sealed record class SyntaxList<T>(ImmutableArray<T> Nodes) : SyntaxNode, IReadOnlyList<T>
    where T : SyntaxNode
{
    public override IEnumerable<SyntaxNode> Chilren => Nodes;

    public int Count => Nodes.Length;

    public T this[int index] => Nodes[index];

    public static implicit operator SyntaxList<T>(ImmutableArray<T> nodes) => new SyntaxList<T>(nodes);
    public override string ToString() => $"[{string.Join(", ", Nodes)}]";
    public IEnumerator<T> GetEnumerator() => Nodes.AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public abstract record class DeclarationSyntax : StatementSyntax;

public abstract record class StatementSyntax : SyntaxNode;

public abstract record class ExpressionSyntax : SyntaxNode;

public abstract record class TypeSyntax : SyntaxNode;

public sealed record class ProgramSyntax(SyntaxList<DeclarationSyntax> Declarations) : SyntaxNode
{
    public override IEnumerable<SyntaxNode> Chilren => Declarations;
}

public sealed record class ExpressionStatementSyntax(ExpressionSyntax Expression, Token Semicolon) : StatementSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Expression;
            yield return Semicolon;
        }
    }
}

public sealed record class FunctionDeclarationSyntax(
    Token KeywordFunc,
    Token Name,
    Token ParenOpen,
    SeparatedSyntaxList<ParameterSyntax> Parameters,
    Token ParenClose,
    TypeSpecifierSyntax? ReturnType,
    BlockStatementSyntax Block
    ) : DeclarationSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return KeywordFunc;
            yield return Name;
            yield return ParenOpen;
            yield return Parameters;
            yield return ParenClose;
            if (ReturnType is not null) yield return ReturnType;
            yield return Block;
        }
    }
}

public sealed record class VariableDeclarationSyntax(
    Token KeywordVar,
    Token Name,
    TypeSpecifierSyntax? Type,
    ValueSpecifierSyntax? Value,
    Token Semicolon
    ) : DeclarationSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return KeywordVar;
            yield return Name;
            if (Type is not null) yield return Type;
            if (Value is not null) yield return Value;
            yield return Semicolon;
        }
    }
}

public sealed record class IfStatementSyntax(
    Token KeywordIf,
    Token ParenOpen,
    ExpressionSyntax Condition,
    Token ParenClose,
    StatementSyntax Then,
    ElseSyntax? Else
    ) : StatementSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return KeywordIf;
            yield return ParenOpen;
            yield return Condition;
            yield return ParenClose;
            yield return Then;
            if (Else is not null) yield return Else;
        }
    }
}

public sealed record class WhileStatementSyntax(
    Token KeywordWhile,
    Token ParenOpen,
    ExpressionSyntax Condition,
    Token ParenClose,
    StatementSyntax Body
    ) : StatementSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return KeywordWhile;
            yield return ParenOpen;
            yield return Condition;
            yield return ParenClose;
            yield return Body;
        }
    }
}

public sealed record class ReturnStatementSyntax(
    Token KeywordReturn,
    ExpressionSyntax? Value,
    Token Semicolon) : StatementSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return KeywordReturn;
            if (Value is not null) yield return Value;
            yield return Semicolon;
        }
    }
}

public sealed record class ElseSyntax(
    Token KeywordElse,
    StatementSyntax Body) : SyntaxNode
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return KeywordElse;
            yield return Body;
        }
    }
}

public sealed record class CallExpressionSyntax(
    ExpressionSyntax Function,
    Token ParenOpen,
    SeparatedSyntaxList<ExpressionSyntax> Args,
    Token ParenClose) : ExpressionSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Function;
            yield return ParenOpen;
            yield return Args;
            yield return ParenClose;
        }
    }
}

public sealed record class GroupExpressionSyntax(
    Token ParenOpen,
    ExpressionSyntax Subexpression,
    Token ParenClose) : ExpressionSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return ParenOpen;
            yield return Subexpression;
            yield return ParenClose;
        }
    }
}

public sealed record class LiteralExpressionSyntax(Token Value) : ExpressionSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Value;
        }
    }
}

public sealed record class BinaryExpressionSyntax(
    ExpressionSyntax Left,
    Token Op,
    ExpressionSyntax Right) : ExpressionSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Left;
            yield return Op;
            yield return Right;
        }
    }
}

public sealed record class PrefixUnaryExpressionSyntax(Token Op, ExpressionSyntax Right) : ExpressionSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Op;
            yield return Right;
        }
    }
}

public sealed record class PostfixUnaryExpressionSyntax(ExpressionSyntax Left, Token Op) : ExpressionSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Left;
            yield return Op;
        }
    }
}

public sealed record class NameExpressionSyntax(Token Name) : ExpressionSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Name;
        }
    }
}

public sealed record class ParameterSyntax(Token Name, Token Colon, TypeSyntax Type) : SyntaxNode
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Name;
            yield return Colon;
            yield return Type;
        }
    }
}

public sealed record class TypeSpecifierSyntax(Token Colon, TypeSyntax Type) : SyntaxNode
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Colon;
            yield return Type;
        }
    }
}

public sealed record class ValueSpecifierSyntax(Token Assign, ExpressionSyntax Value) : SyntaxNode
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Assign;
            yield return Value;
        }
    }
}

public sealed record class BlockStatementSyntax(
    Token BraceOpen,
    SyntaxList<StatementSyntax> Statements,
    Token BraceClose) : StatementSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return BraceOpen;
            yield return Statements;
            yield return BraceClose;
        }
    }
}

public sealed record class NameTypeSyntax(Token Name) : TypeSyntax
{
    public override IEnumerable<SyntaxNode> Chilren
    {
        get
        {
            yield return Name;
        }
    }
}
