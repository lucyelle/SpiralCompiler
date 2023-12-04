using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace SpiralCompiler.Syntax;

public sealed class Parser
{
    private readonly IEnumerator<Token> tokens;
    private readonly List<Token> peekBuffer = new();
    private readonly List<ErrorMessage> errors = new();

    public Parser(IEnumerable<Token> tokens)
    {
        this.tokens = tokens.GetEnumerator();
    }

    public static ParseTree Parse(IEnumerable<Token> tokens)
    {
        var parser = new Parser(tokens);
        return new(parser.ParseProgram(), parser.errors);
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
        var token = Peek();
        if (token.Type is TokenType.KeywordFunc)
        {
            return ParseFunctionDeclaration();
        }
        else if (token.Type is TokenType.KeywordVar)
        {
            return ParseVariableDeclaration();
        }
        else if (token.Type is TokenType.KeywordInterface)
        {
            return ParseInterfaceDeclaration();
        }
        else if (token.Type is TokenType.KeywordClass)
        {
            return ParseClassDeclaration();
        }
        else
        {
            ReportError($"unexpected token {token.Type} while parsing declaration");
            return new UnexpectedDeclarationSyntax(Synchronize(
                TokenType.KeywordFunc,
                TokenType.KeywordInterface,
                TokenType.KeywordClass,
                TokenType.KeywordVar));
        }
    }

    private DeclarationSyntax ParseVariableDeclaration()
    {
        var keywordVar = Expect(TokenType.KeywordVar);
        var name = Expect(TokenType.Identifier);
        TypeSpecifierSyntax? typeSpecifier = null;
        ValueSpecifierSyntax? valueSpecifier = null;

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

    private DeclarationSyntax ParseInterfaceDeclaration()
    {
        var keywordInterface = Expect(TokenType.KeywordInterface);
        var name = Expect(TokenType.Identifier);
        var signatures = ImmutableArray.CreateBuilder<MethodSignatureSyntax>();

        BaseSpecifierSyntax? bases = null;
        if (Peek().Type == TokenType.Colon) bases = ParseBaseSpecifier();

        var openBrace = Expect(TokenType.BraceOpen);
        while (Peek().Type != TokenType.BraceClose)
        {
            signatures.Add(ParseMethodSignature());
        }
        var closeBrace = Expect(TokenType.BraceClose);

        return new InterfaceDeclarationSyntax(keywordInterface, name, bases, openBrace, signatures.ToImmutable(), closeBrace);
    }

    private MethodSignatureSyntax ParseMethodSignature()
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

        var semicol = Expect(TokenType.Semicolon);

        return new MethodSignatureSyntax(keywordFunc, name, parenOpen, parameters, parenClose, returnSpecifier, semicol);
    }

    private DeclarationSyntax ParseClassDeclaration()
    {
        var keywordClass = Expect(TokenType.KeywordClass);
        var name = Expect(TokenType.Identifier);
        var members = ImmutableArray.CreateBuilder<DeclarationSyntax>();

        BaseSpecifierSyntax? bases = null;
        if (Peek().Type == TokenType.Colon) bases = ParseBaseSpecifier();

        var openBrace = Expect(TokenType.BraceOpen);
        while (Peek().Type != TokenType.BraceClose)
        {
            members.Add(ParseClassMemberDeclaration());
        }
        var closeBrace = Expect(TokenType.BraceClose);

        return new ClassDeclarationSyntax(keywordClass, name, bases, openBrace, members.ToImmutable(), closeBrace);
    }

    private DeclarationSyntax ParseClassMemberDeclaration()
    {
        var token = Peek();
        if (token.Type is TokenType.KeywordFunc)
        {
            return ParseFunctionDeclaration();
        }
        else if (token.Type is TokenType.KeywordField)
        {
            return ParseFieldDeclaration();
        }
        else if (token.Type is TokenType.KeywordCtor)
        {
            return ParseCtorDeclaration();
        }
        else
        {
            ReportError($"unexpected token {token.Type} while parsing class member");
            return new UnexpectedDeclarationSyntax(Synchronize(
                TokenType.KeywordFunc,
                TokenType.KeywordInterface,
                TokenType.KeywordClass,
                TokenType.KeywordVar,
                TokenType.KeywordField,
                TokenType.KeywordCtor));
        }
    }

    private DeclarationSyntax ParseFieldDeclaration()
    {
        var keywordField = Expect(TokenType.KeywordField);
        var name = Expect(TokenType.Identifier);
        var colon = Expect(TokenType.Colon);
        var type = ParseType();
        var semicol = Expect(TokenType.Semicolon);
        return new FieldDeclarationSyntax(keywordField, name, colon, type, semicol);
    }

    private DeclarationSyntax ParseCtorDeclaration()
    {
        var keywordCtor = Expect(TokenType.KeywordCtor);
        var parenOpen = Expect(TokenType.ParenOpen);

        var parameters = ParseSeparated(ParseParameter, TokenType.Comma, TokenType.ParenClose);

        var parenClose = Expect(TokenType.ParenClose);

        // Body
        var body = ParseBlockStatement();

        return new CtorDeclarationSyntax(keywordCtor, parenOpen, parameters, parenClose, body);
    }

    private BaseSpecifierSyntax ParseBaseSpecifier()
    {
        var colon = Expect(TokenType.Colon);
        var parts = ImmutableArray.CreateBuilder<SyntaxNode>();
        parts.Add(ParseType());
        while (this.Matches(TokenType.Comma, out var comma))
        {
            parts.Add(comma);
            parts.Add(ParseType());
        }
        return new BaseSpecifierSyntax(colon, new(parts.ToImmutable()));
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
        if (type == TokenType.Plus ||
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

        // Member access
        if (type == TokenType.Dot)
        {
            var dot = Consume();
            var memberName = Expect(TokenType.Identifier);
            left = new MemberExpressionSyntax(left, dot, memberName);
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

        // Indexing
        if (type == TokenType.BracketOpen)
        {
            var bracketOpen = Consume();
            var args = ParseSeparated(ParseExpression, TokenType.Comma, TokenType.ParenClose);
            var bracketClose = Expect(TokenType.BracketClose);
            left = new IndexExpressionSyntax(left, bracketOpen, args, bracketClose);
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
            var newClass = ParseType();
            parenOpen = Expect(TokenType.ParenOpen);
            var args = ParseSeparated(ParseExpression, TokenType.Comma, TokenType.ParenClose);
            var parenClose = Expect(TokenType.ParenClose);
            return new NewExpressionSyntax(keywordNew, newClass, parenOpen, args, parenClose);
        }

        ReportError($"unexpexted token {Peek().Text} while parsing expression");
        return new UnexpectedExpressionSyntax(Synchronize(
            TokenType.Semicolon,
            TokenType.Colon,
            TokenType.BraceOpen,
            TokenType.Comma,
            TokenType.BraceClose,
            TokenType.ParenOpen,
            TokenType.ParenClose));
    }

    private TypeSyntax ParseType()
    {
        if (Matches(TokenType.Identifier, out var name))
        {
            return new NameTypeSyntax(name);
        }

        ReportError($"unexpexted token {Peek().Text} while parsing type reference");
        return new UnexpectedTypeSyntax(Synchronize(
            TokenType.Semicolon,
            TokenType.Colon,
            TokenType.BraceOpen,
            TokenType.Comma));
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

    private ImmutableArray<SyntaxNode> Synchronize(params TokenType[] stoppers)
    {
        var result = ImmutableArray.CreateBuilder<SyntaxNode>();
        while (true)
        {
            var token = Peek();
            if (token.Type == TokenType.EndOfFile) break;
            if (stoppers.Contains(token.Type)) break;
            result.Add(Consume());
        }
        return result.ToImmutable();
    }

    private Token Expect(TokenType type)
    {
        var token = Peek();
        if (token.Type != type)
        {
            ReportError($"expected token of type {type}, but got {token.Type}");
            return new Token(string.Empty, type, token.Range);
        }
        return Consume();
    }

    private void ReportError(string message)
    {
        var pos = Peek().Range;
        errors.Add(new ErrorMessage(message, pos));
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
