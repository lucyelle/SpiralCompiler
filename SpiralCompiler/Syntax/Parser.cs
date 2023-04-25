using System.Diagnostics.CodeAnalysis;

namespace SpiralCompiler.Syntax;

public sealed class Parser
{
    private readonly IEnumerator<Token> tokens;
    private readonly List<Token> peekBuffer = new();

    public Parser(IEnumerable<Token> tokens)
    {
        this.tokens = tokens.GetEnumerator();
    }

    public static Statement Parse(IEnumerable<Token> tokens)
    {
        var parser = new Parser(tokens);
        return parser.ParseProgram();
    }

    public Statement ParseProgram()
    {
        var statements = new List<Statement>();
        while (!Matches(TokenType.EndOfFile))
        {
            statements.Add(ParseStatement());
        }
        return new Statement.Block(statements);
    }

    private Statement ParseStatement()
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
            return ParseVarStatement();
        }
        else if (token.Type is TokenType.KeywordFor)
        {
            return ParseForStatement();
        }
        else if (token.Type is TokenType.KeywordReturn)
        {
            return ParseReturnStatement();
        }
        else if (token.Type is TokenType.KeywordPrivate or TokenType.KeywordPublic)
        {
            return ParseModifier();
        }
        else
        {
            var expr = ParseExpression();
            Expect(TokenType.Semicolon);
            return new Statement.Expr(expr);
        }
    }

    private Statement ParseModifier()
    {
        var token = Consume();
        Visibility visibility;

        if (token.Type is TokenType.KeywordPublic)
        {
            visibility = Visibility.Public;
        }
        else
        {
            visibility = Visibility.Private;
        }

        var nextToken = Peek();
        if (nextToken.Type is TokenType.KeywordClass)
        {
            return ParseClassStatement(visibility);
        }
        else if (nextToken.Type is TokenType.KeywordField)
        {
            return ParseFieldStatement(visibility);
        }
        else if (nextToken.Type is TokenType.KeywordFunc)
        {
            return ParseFunctionDefinitonStatement(visibility);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private Statement ParseFieldStatement(Visibility visibility)
    {
        Expect(TokenType.KeywordField);
        var name = Expect(TokenType.Identifier).Text;
        TypeReference? type = null;
        Expression? value = null;

        if (Matches(TokenType.Semicolon)) throw new InvalidOperationException("no type or value given to variable");

        if (Matches(TokenType.Colon))
        {
            type = ParseTypeReference();
        }

        if (Matches(TokenType.Assign))
        {
            value = ParseExpression();
        }
        Expect(TokenType.Semicolon);

        return new Statement.Field(visibility, name, type, value);
    }

    private Statement ParseClassStatement(Visibility visibility)
    {
        Expect(TokenType.KeywordClass);
        var name = Expect(TokenType.Identifier).Text;

        var bases = new List<string>();

        // Bases
        while (Matches(TokenType.Colon))
        {
            bases.Add(Expect(TokenType.Identifier).Text);
        }

        Expect(TokenType.BraceOpen);

        var fields = new List<Statement.Field>();
        var functions = new List<Statement.FunctionDef>();

        // Fields, functions
        while (Matches(TokenType.KeywordPublic) || Matches(TokenType.KeywordPrivate))
        {
            if (Matches(TokenType.KeywordField))
            {
                fields.Add((Statement.Field)ParseFieldStatement(visibility));
            }
            else if (Matches(TokenType.KeywordFunc))
            {
                functions.Add((Statement.FunctionDef)ParseFunctionDefinitonStatement(visibility));
            }
        }

        return new Statement.Class(visibility, name, fields, functions, bases);
    }

    private Statement ParseReturnStatement()
    {
        Expect(TokenType.KeywordReturn);
        Expression? returnValue = null;
        if (!Matches(TokenType.Semicolon))
        {
            returnValue = ParseExpression();
            Expect(TokenType.Semicolon);
        }
        return new Statement.Return(returnValue);
    }

    private Statement ParseIfStatement()
    {
        Expect(TokenType.KeywordIf);

        // Condition
        Expect(TokenType.ParenOpen);
        var condition = ParseExpression();
        Expect(TokenType.ParenClose);

        // Body
        var body = ParseStatement();

        // Else
        Statement? elseBody = null;
        if (Matches(TokenType.KeywordElse))
        {
            elseBody = ParseStatement();
        }

        return new Statement.If(condition, body, elseBody);
    }

    private Statement ParseVarStatement()
    {
        Expect(TokenType.KeywordVar);
        var name = Expect(TokenType.Identifier).Text;
        TypeReference? type = null;
        Expression? value = null;

        if (Matches(TokenType.Semicolon)) throw new InvalidOperationException("no type or value given to variable");

        if (Matches(TokenType.Colon))
        {
            type = ParseTypeReference();
        }

        if (Matches(TokenType.Assign))
        {
            value = ParseExpression();
        }
        Expect(TokenType.Semicolon);

        return new Statement.Var(name, type, value);
    }

    private Statement ParseForStatement()
    {
        // Range
        Expect(TokenType.KeywordFor);
        Expect(TokenType.ParenOpen);
        var iterator = Expect(TokenType.Identifier).Text;
        Expect(TokenType.Range);
        var range = ParseExpression();
        Expect(TokenType.ParenClose);
        // Body
        var body = ParseBlockStatement();

        return new Statement.For(iterator, range, body);
    }

    private Statement ParseFunctionDefinitonStatement(Visibility visibility)
    {
        Expect(TokenType.KeywordFunc);
        var name = Expect(TokenType.Identifier).Text;
        Expect(TokenType.ParenOpen);

        // Parameters
        var parameters = new List<Parameter>();

        while (Peek().Type != TokenType.ParenClose)
        {
            parameters.Add(ParseParameter());
            if (!Matches(TokenType.Comma)) break;
        }
        Expect(TokenType.ParenClose);

        // Return type
        TypeReference? returnType = null;
        if (Matches(TokenType.Colon))
        {
            returnType = ParseTypeReference();
        }

        // Body
        var body = ParseBlockStatement();

        return new Statement.FunctionDef(visibility, name, parameters, returnType, body);
    }

    private Parameter ParseParameter()
    {
        var name = Expect(TokenType.Identifier).Text;
        Expect(TokenType.Colon);
        var type = ParseTypeReference();

        return new Parameter(name, type);
    }

    private Statement ParseWhileStatement()
    {
        Expect(TokenType.KeywordWhile);

        // Condition
        Expect(TokenType.ParenOpen);
        var condition = ParseExpression();
        Expect(TokenType.ParenClose);

        // Body
        var body = ParseStatement();

        return new Statement.While(condition, body);
    }

    private Statement ParseBlockStatement()
    {
        Expect(TokenType.BraceOpen);
        var statements = new List<Statement>();
        while (Peek().Type != TokenType.BraceClose)
        {
            statements.Add(ParseStatement());
        }
        Expect(TokenType.BraceClose);

        return new Statement.Block(statements);
    }

    private Expression ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private Expression ParseAssignmentExpression()
    {
        var left = ParseOrExpression();
        var type = Peek().Type;
        if (type == TokenType.Assign ||
            type == TokenType.AddAssign ||
            type == TokenType.SubtractAssign ||
            type == TokenType.MultiplyAssign ||
            type == TokenType.DivideAssign)
        {
            var op = TranslateBinaryOperator(Consume().Type);
            var right = ParseAssignmentExpression();
            return new Expression.Binary(left, op, right);
        }
        return left;
    }

    private Expression ParseOrExpression()
    {
        var left = ParseAndExpression();
        while (Peek().Type == TokenType.Or)
        {
            Consume();
            var right = ParseAndExpression();
            left = new Expression.Binary(left, BinOp.Or, right);
        }
        return left;
    }

    private Expression ParseAndExpression()
    {
        var left = ParseEqualsExpression();
        while (Peek().Type == TokenType.And)
        {
            Consume();
            var right = ParseEqualsExpression();
            left = new Expression.Binary(left, BinOp.And, right);
        }
        return left;
    }

    private Expression ParseEqualsExpression()
    {
        var left = ParseLessThanExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.Equals || type == TokenType.NotEqual)
        {
            var op = TranslateBinaryOperator(Consume().Type);
            var right = ParseLessThanExpression();
            left = new Expression.Binary(left, op, right);
            goto peek;
        }
        return left;
    }

    private Expression ParseLessThanExpression()
    {
        var left = ParseAddExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.LessThan ||
            type == TokenType.GreaterThan ||
            type == TokenType.LessEquals ||
            type == TokenType.GreaterEquals)
        {
            var op = TranslateBinaryOperator(Consume().Type);
            var right = ParseAddExpression();
            left = new Expression.Binary(left, op, right);
            goto peek;
        }
        return left;
    }

    private Expression ParseAddExpression()
    {
        var left = ParseMultiplyExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.Plus || type == TokenType.Minus)
        {
            var op = TranslateBinaryOperator(Consume().Type);
            var right = ParseMultiplyExpression();
            left = new Expression.Binary(left, op, right);
            goto peek;
        }
        return left;
    }

    private Expression ParseMultiplyExpression()
    {
        var left = ParsePrefixExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.Multiply || type == TokenType.Divide || type == TokenType.Modulo)
        {
            var op = TranslateBinaryOperator(Consume().Type);
            var right = ParsePrefixExpression();
            left = new Expression.Binary(left, op, right);
            goto peek;
        }
        return left;
    }

    private Expression ParsePrefixExpression()
    {
        var type = Peek().Type;
        if (type == TokenType.Increment ||
            type == TokenType.Decrement ||
            type == TokenType.Plus ||
            type == TokenType.Minus ||
            type == TokenType.Not)
        {
            var op = TranslateUnaryPreOperator(Consume().Type);
            var right = ParsePrefixExpression();
            return new Expression.UnaryPre(op, right);
        }
        return ParsePostfixExpression();
    }

    private Expression ParsePostfixExpression()
    {
        var left = ParseAtomExpression();
    peek:
        var type = Peek().Type;
        if (type == TokenType.Increment || type == TokenType.Decrement)
        {
            var op = TranslateUnaryPostOperator(Consume().Type);
            left = new Expression.UnaryPost(left, op);
            goto peek;
        }

        // Function call
        if (type == TokenType.ParenOpen)
        {
            Consume();
            var args = new List<Expression>();
            while (Peek().Type != TokenType.ParenClose)
            {
                args.Add(ParseExpression());
                if (!Matches(TokenType.Comma)) break;
            }
            Expect(TokenType.ParenClose);
            left = new Expression.FunctionCall(left, args);
            goto peek;
        }
        return left;
    }

    private string EscapeStringLiteral(string tokenStr)
    {
        // TODO: new line ...
        return tokenStr.Substring(1, tokenStr.Length - 2);
    }

    private string EscapeChar(char ch)
    {
        throw new NotImplementedException();
    }

    private Expression ParseAtomExpression()
    {
        var type = Peek().Type;
        if (type == TokenType.String)
        {
            return new Expression.String(EscapeStringLiteral(Consume().Text));
        }
        if (type == TokenType.Integer)
        {
            return new Expression.Integer(int.Parse(Consume().Text));
        }
        if (type == TokenType.Double)
        {
            return new Expression.Double(double.Parse(Consume().Text));
        }
        if (type == TokenType.Boolean)
        {
            return new Expression.Boolean(bool.Parse(Consume().Text));
        }
        if (type == TokenType.Identifier)
        {
            return new Expression.Identifier(Consume().Text);
        }

        if (type == TokenType.ParenOpen)
        {
            Consume();
            var expr = ParseExpression();
            Expect(TokenType.ParenClose);
            return expr;
        }

        throw new InvalidOperationException($"unexpexted token {Peek().Text} while parsing expression");
    }

    private static BinOp TranslateBinaryOperator(TokenType type) => type switch
    {
        TokenType.Plus => BinOp.Add,
        TokenType.Minus => BinOp.Substract,
        TokenType.Multiply => BinOp.Multiply,
        TokenType.Divide => BinOp.Divide,
        TokenType.Assign => BinOp.Assign,
        TokenType.AddAssign => BinOp.AddAssign,
        TokenType.SubtractAssign => BinOp.SubtractAssign,
        TokenType.MultiplyAssign => BinOp.MultiplyAssign,
        TokenType.DivideAssign => BinOp.DivideAssign,
        TokenType.Equals => BinOp.Equals,
        TokenType.NotEqual => BinOp.NotEqual,
        TokenType.LessEquals => BinOp.LessEquals,
        TokenType.LessThan => BinOp.Less,
        TokenType.GreaterEquals => BinOp.GreaterEquals,
        TokenType.GreaterThan => BinOp.Greater,
        TokenType.And => BinOp.And,
        TokenType.Or => BinOp.Or,
        TokenType.Modulo => BinOp.Modulo,
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    private static UnOpPre TranslateUnaryPreOperator(TokenType type) => type switch
    {
        TokenType.Increment => UnOpPre.Increment,
        TokenType.Decrement => UnOpPre.Decrement,
        TokenType.Plus => UnOpPre.Plus,
        TokenType.Minus => UnOpPre.Minus,
        TokenType.Not => UnOpPre.Not,
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    private static UnOpPost TranslateUnaryPostOperator(TokenType type) => type switch
    {
        TokenType.Increment => UnOpPost.Increment,
        TokenType.Decrement => UnOpPost.Decrement,
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    private TypeReference ParseTypeReference()
    {
        if (Peek().Type == TokenType.Identifier)
        {
            return new TypeReference.Identifier(Consume().Text);
        }

        throw new InvalidOperationException($"unexpexted token {Peek().Text} while parsing type reference");
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
