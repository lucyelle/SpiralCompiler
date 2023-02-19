using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.Syntax;

internal class Parser
{
    private readonly IEnumerator<Token> tokens;
	private readonly List<Token> peekBuffer = new();

	public Parser(IEnumerable<Token> tokens)
	{
		this.tokens = tokens.GetEnumerator();
	}

	private Statement ParseStatement()
	{
		var token = Peek();
		if (token.Type == TokenType.KeywordIf)
		{
			return ParseIfStatement();
		}
		else if (token.Type == TokenType.KeywordWhile)
		{
            return ParseWhileStatement();
        }
		else if (token.Type == TokenType.BraceOpen)
		{
			return ParseBlockStatement();
		}
		else
		{
			throw new NotImplementedException("Unknown token for statement");
		}
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
		throw new NotImplementedException();
	}

    private TypeReference ParseTypeReference()
    {
        throw new NotImplementedException();
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
