using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SpiralCompiler.Syntax;

public sealed class Parser
{
    private readonly IEnumerator<Token> tokens;
    private readonly List<Token> peekBuffer = new();

    public Parser(IEnumerable<Token> tokens)
    {
        this.tokens = tokens.GetEnumerator();
    }

    public static ProgramSyntax Parse(IEnumerable<Token> tokens)
    {
        var parser = new Parser(tokens);
        return parser.ParseProgram();
    }

    public ProgramSyntax ParseProgram()
    {
        var statements = ImmutableArray.CreateBuilder<DeclarationSyntax>();
        while (!Matches(TokenType.EndOfFile))
        {
            statements.Add(ParseDeclaration());
        }
        return new ProgramSyntax(statements.ToImmutable());
    }

    private DeclarationSyntax ParseDeclaration()
    {
        throw new NotImplementedException();
    }

    private DeclarationSyntax ParseVariableDeclaration()
    {
        var keywordVar = Expect(TokenType.KeywordVar);
        var name = Expect(TokenType.Identifier);
        TypeSpecifierSyntax? typeSpecifier = null;
        ValueSpecifierSyntax? valueSpecifier = null;

        if (Matches(TokenType.Semicolon)) throw new InvalidOperationException("no type or value given to variable");

        if (Matches(TokenType.Colon, out var colon))
        {
            var type = ParseType();
            typeSpecifier = new(colon, type);
        }

        if (Matches(TokenType.Assign, out var assign))
        {
            var value = ParseExpression();
            valueSpecifier = new(assign, value);
        }
        var semicolon = Expect(TokenType.Semicolon);

        return new VariableDeclarationSyntax(keywordVar, name, typeSpecifier, valueSpecifier, semicolon);
    }

    private DeclarationSyntax ParseFunctionDeclaration()
    {
        var keywordFunc = Expect(TokenType.KeywordFunc);
        var name = Expect(TokenType.Identifier);
        var parenOpen = Expect(TokenType.ParenOpen);

        var parameters = ParseSeparated(ParseParameter, TokenType.Comma, TokenType.ParenClose);

        var parenClose = Expect(TokenType.ParenClose);

        // Return type
        TypeSpecifierSyntax? returnSpecifier = null;
        if (Matches(TokenType.Colon, out var colon))
        {
            var returnType = ParseType();
            returnSpecifier = new(colon, returnType);
        }

        // Body
        var body = ParseBlockStatement();

        return new FunctionDeclarationSyntax(keywordFunc, name, parenOpen, parameters, parenClose, returnSpecifier, body);
    }

    private ParameterSyntax ParseParameter()
    {
        var name = Expect(TokenType.Identifier);
        var colon = Expect(TokenType.Colon);
        var type = ParseType();

        return new ParameterSyntax(name, colon, type);
    }

    private StatementSyntax ParseStatement()
    {
        var token = Peek();
        if (token.Type is TokenType.KeywordIf)
        {
            return ParseIfStatement();
        }
        else if (token.Type is TokenType.KeywordWhile)
        {
            return ParseWhileStatement();
        }
        else if (token.Type is TokenType.BraceOpen)
        {
            return ParseBlockStatement();
        }
        else if (token.Type is TokenType.KeywordVar)
        {
            return ParseVariableDeclaration();
        }
        else if (token.Type is TokenType.KeywordReturn)
        {
            return ParseReturnStatement();
        }
        else
        {
            var expr = ParseExpression();
            var semicolon = Expect(TokenType.Semicolon);
            return new ExpressionStatementSyntax(expr, semicolon);
        }
    }

    private StatementSyntax ParseReturnStatement()
    {
        var keywordReturn = Expect(TokenType.KeywordReturn);
        ExpressionSyntax? returnValue = null;
        if (Peek().Type != TokenType.Semicolon)
        {
            returnValue = ParseExpression();
        }

        var semicolon = Expect(TokenType.Semicolon);
        return new ReturnStatementSyntax(keywordReturn, returnValue, semicolon);
    }

    private StatementSyntax ParseIfStatement()
    {
        var keywordIf = Expect(TokenType.KeywordIf);

        // Condition
        var parenOpen = Expect(TokenType.ParenOpen);
        var condition = ParseExpression();
        var parenClose = Expect(TokenType.ParenClose);

        // Body
        var body = ParseStatement();

        // Else
        ElseSyntax? @else = null;
        if (Matches(TokenType.KeywordElse, out var keywordElse))
        {
            var elseBody = ParseStatement();
            @else = new ElseSyntax(keywordElse, elseBody);
        }

        return new IfStatementSyntax(keywordIf, parenOpen, condition, parenClose, body, @else);
    }

    private StatementSyntax ParseWhileStatement()
    {
        var keywordWhile = Expect(TokenType.KeywordWhile);

        // Condition
        var parenOpen = Expect(TokenType.ParenOpen);
        var condition = ParseExpression();
        var parenClose = Expect(TokenType.ParenClose);

        // Body
        var body = ParseStatement();

        return new WhileStatementSyntax(keywordWhile, parenOpen, condition, parenClose, body);
    }

    private BlockStatementSyntax ParseBlockStatement()
    {
        var braceOpen = Expect(TokenType.BraceOpen);
        var statements = ImmutableArray.CreateBuilder<StatementSyntax>();
        while (Peek().Type != TokenType.BraceClose)
        {
            statements.Add(ParseStatement());
        }
        var braceClose = Expect(TokenType.BraceClose);

        return new BlockStatementSyntax(braceOpen, statements.ToImmutable(), braceClose);
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private ExpressionSyntax ParseAssignmentExpression()
    {
        var left = ParseOrExpression();
        var type = Peek().Type;
        if (type == TokenType.Assign ||
            type == TokenType.AddAssign ||
            type == TokenType.SubtractAssign ||
            type == TokenType.MultiplyAssign ||
            type == TokenType.DivideAssign)
        {
            var op = Consume();
            var right = ParseAssignmentExpression();
            return new BinaryExpressionSyntax(left, op, right);
        }
        return left;
    }

    private ExpressionSyntax ParseOrExpression()
    {
        var left = ParseAndExpression();
        while (Peek().Type == TokenType.Or)
        {
            var op = Consume();
            var right = ParseAndExpression();
            left = new BinaryExpressionSyntax(left, op, right);
        }
        return left;
    }

    private ExpressionSyntax ParseAndExpression()
    {
        var left = ParseEqualsExpression();
        while (Peek().Type == TokenType.And)
        {
            var op = Consume();
            var right = ParseEqualsExpression();
            left = new BinaryExpressionSyntax(left, op, right);
        }
        return left;
    }

    private ExpressionSyntax ParseEqualsExpression()
    {
        var left = ParseLessThanExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.Equals || type == TokenType.NotEqual)
        {
            var op = Consume();
            var right = ParseLessThanExpression();
            left = new BinaryExpressionSyntax(left, op, right);
            goto peek;
        }
        return left;
    }

    private ExpressionSyntax ParseLessThanExpression()
    {
        var left = ParseAddExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.LessThan ||
            type == TokenType.GreaterThan ||
            type == TokenType.LessEquals ||
            type == TokenType.GreaterEquals)
        {
            var op = Consume();
            var right = ParseAddExpression();
            left = new BinaryExpressionSyntax(left, op, right);
            goto peek;
        }
        return left;
    }

    private ExpressionSyntax ParseAddExpression()
    {
        var left = ParseMultiplyExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.Plus || type == TokenType.Minus)
        {
            var op = Consume();
            var right = ParseMultiplyExpression();
            left = new BinaryExpressionSyntax(left, op, right);
            goto peek;
        }
        return left;
    }

    private ExpressionSyntax ParseMultiplyExpression()
    {
        var left = ParsePrefixExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.Multiply || type == TokenType.Divide || type == TokenType.Modulo)
        {
            var op = Consume();
            var right = ParsePrefixExpression();
            left = new BinaryExpressionSyntax(left, op, right);
            goto peek;
        }
        return left;
    }

    private ExpressionSyntax ParsePrefixExpression()
    {
        var type = Peek().Type;
        if (type == TokenType.Increment ||
            type == TokenType.Decrement ||
            type == TokenType.Plus ||
            type == TokenType.Minus ||
            type == TokenType.Not)
        {
            var op = Consume();
            var right = ParsePrefixExpression();
            return new PrefixUnaryExpressionSyntax(op, right);
        }
        return ParsePostfixExpression();
    }

    private ExpressionSyntax ParsePostfixExpression()
    {
        var left = ParseAtomExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.Increment || type == TokenType.Decrement)
        {
            var op = Consume();
            left = new PostfixUnaryExpressionSyntax(left, op);
            goto peek;
        }

        // Member access
        if (type == TokenType.Dot)
        {
            var dot = Consume();
            var memberName = Expect(TokenType.Identifier);
            // TODO
            throw new NotImplementedException();
            // left = new Expression.MemberAccess(left, memberName);
            goto peek;
        }

        // Function call
        if (type == TokenType.ParenOpen)
        {
            var parenOpen = Consume();
            var args = ParseSeparated(ParseExpression, TokenType.Comma, TokenType.ParenClose);
            var parenClose = Expect(TokenType.ParenClose);
            left = new CallExpressionSyntax(left, parenOpen, args, parenClose);
            goto peek;
        }
        return left;
    }

    private ExpressionSyntax ParseAtomExpression()
    {
        if (Peek().Type is TokenType.String
                        or TokenType.Integer
                        or TokenType.Double
                        or TokenType.Boolean)
        {
            return new LiteralExpressionSyntax(Consume());
        }

        if (Matches(TokenType.Identifier, out var name))
        {
            return new NameExpressionSyntax(name);
        }

        if (Matches(TokenType.ParenOpen, out var parenOpen))
        {
            var expr = ParseExpression();
            var parenClose = Expect(TokenType.ParenClose);
            return new GroupExpressionSyntax(parenOpen, expr, parenClose);
        }

        if (Matches(TokenType.KeywordNew, out var keywordNew))
        {
            // TODO
            var newClass = Expect(TokenType.Identifier);
            parenOpen = Expect(TokenType.ParenOpen);
            var parenClose = Expect(TokenType.ParenClose);
            throw new NotImplementedException();
        }

        throw new InvalidOperationException($"unexpexted token {Peek().Text} while parsing expression");
    }

    private TypeSyntax ParseType()
    {
        if (Matches(TokenType.Identifier, out var name))
        {
            return new NameTypeSyntax(name);
        }

        throw new InvalidOperationException($"unexpexted token {Peek().Text} while parsing type reference");
    }

    private SeparatedSyntaxList<T> ParseSeparated<T>(Func<T> elementParser, TokenType separator, TokenType stop)
        where T : SyntaxNode
    {
        if (Peek().Type == stop)
        {
            return new(ImmutableArray<SyntaxNode>.Empty);
        }

        var nodes = ImmutableArray.CreateBuilder<SyntaxNode>();
        var first = elementParser();
        nodes.Add(first);
        while (Matches(separator, out var separatorToken))
        {
            nodes.Add(separatorToken);
            var next = elementParser();
            nodes.Add(next);
        }

        return new(nodes.ToImmutable());
    }

    private Token Expect(TokenType type)
    {
        var token = Peek();
        if (token.Type != type) throw new InvalidOperationException($"expected token of type {type}, but got {token.Type}");
        return Consume();
    }

    private bool Matches(TokenType type) => Matches(type, out _);

    private bool Matches(TokenType type, [MaybeNullWhen(false)] out Token result)
    {
        var token = Peek();
        if (token.Type == type)
        {
            result = Consume();
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    private Token Consume()
    {
        Peek(0);
        var token = peekBuffer[0];
        peekBuffer.RemoveAt(0);
        return token;
    }

    private Token Peek(int offset = 0)
    {
        while (peekBuffer.Count <= offset)
        {
            if (!tokens.MoveNext()) throw new InvalidOperationException("no more tokens");
            peekBuffer.Add(tokens.Current);
        }
        return peekBuffer[offset];
    }
}
