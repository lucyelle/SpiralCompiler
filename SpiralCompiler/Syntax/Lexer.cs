using System.Collections.Generic;

namespace SpiralCompiler.Syntax;

public sealed class Lexer
{
    private readonly string code;
    private int index;

    private bool IsEnd => index >= code.Length;

    private Lexer(string code)
    {
        this.code = code;
    }

    public static IEnumerable<Token> Lex(string code)
    {
        var lexer = new Lexer(code);

        while (true)
        {
            var token = lexer.Next();
            yield return token;

            if (token.Type == TokenType.EndOfFile) break;
        }
    }

    private Token Next()
    {
    begin:
        if (IsEnd) return new Token("", TokenType.EndOfFile, new Range(index, index));

        var ch = Peek();
        if (char.IsWhiteSpace(ch))
        {
            index++;
            goto begin;
        }

        // Comment
        if (ch == '/' && Peek(1) == '/')
        {
            index += 2;
            while (!IsNewline(Peek(0, defaultChar: '\n'))) index++;
            goto begin;
        }

        // String
        if (ch == '"')
        {
            var offset = 1;
            while (Peek(offset, defaultChar: '"') != '"') offset++;
            offset++;
            return Consume(offset, TokenType.String);
        }

        // Braces
        if (ch == '{') return Consume(1, TokenType.BraceOpen);
        if (ch == '}') return Consume(1, TokenType.BraceClose);

        // Parens
        if (ch == '(') return Consume(1, TokenType.ParenOpen);
        if (ch == ')') return Consume(1, TokenType.ParenClose);

        // Brackets
        if (ch == '[') return Consume(1, TokenType.BracketOpen);
        if (ch == ']') return Consume(1, TokenType.BracketClose);

        // Comma
        if (ch == ',') return Consume(1, TokenType.Comma);

        // Semicolon
        if (ch == ';') return Consume(1, TokenType.Semicolon);

        // Colon
        if (ch == ':') return Consume(1, TokenType.Colon);

        // %
        if (ch == '%') return Consume(1, TokenType.Modulo);

        // <
        if (ch == '<')
        {
            // <=
            if (Peek(1) == '=') return Consume(2, TokenType.LessEquals);
            return Consume(1, TokenType.LessThan);
        }

        // >
        if (ch == '>')
        {
            // >=
            if (Peek(1) == '=') return Consume(2, TokenType.GreaterEquals);
            return Consume(1, TokenType.GreaterThan);
        }

        // !=
        if (ch == '!' && Peek(1) == '=') return Consume(2, TokenType.NotEqual);

        // == and =
        if (ch == '=')
        {
            if (Peek(1) == '=') return Consume(2, TokenType.Equals);
            else return Consume(1, TokenType.Assign);
        }

        // +
        if (ch == '+')
        {
            // +=
            if (Peek(1) == '=') return Consume(2, TokenType.AddAssign);
            return Consume(1, TokenType.Plus);
        }

        // -
        if (ch == '-')
        {
            // -=
            if (Peek(1) == '=') return Consume(2, TokenType.SubtractAssign);
            return Consume(1, TokenType.Minus);
        }

        // *
        if (ch == '*')
        {
            // *=
            if (Peek(1) == '=') return Consume(2, TokenType.MultiplyAssign);
            return Consume(1, TokenType.Multiply);
        }

        // /
        if (ch == '/')
        {
            // /=
            if (Peek(1) == '=') return Consume(2, TokenType.DivideAssign);
            return Consume(1, TokenType.Divide);
        }

        // Dot
        if (ch == '.')
        {
            // Range
            if (Peek(1) == '.') return Consume(2, TokenType.Range);
            return Consume(1, TokenType.Dot);
        }

        // Number
        if (char.IsDigit(ch))
        {
            var offset = 1;
            while (char.IsDigit(Peek(offset))) offset++;

            // Double
            if (Peek(offset) == '.' && char.IsDigit(Peek(offset + 1)))
            {
                offset++;
                while (char.IsDigit(Peek(offset))) offset++;
                return Consume(offset, TokenType.Double);
            }
            // Integer
            return Consume(offset, TokenType.Integer);
        }

        // Identifier-like
        if (IsIdent(ch))
        {
            var offset = 1;
            while (IsIdent(Peek(offset))) offset++;
            var text = Consume(offset);

            var tokenType = text switch
            {
                "if" => TokenType.KeywordIf,
                "else" => TokenType.KeywordElse,
                "var" => TokenType.KeywordVar,
                "return" => TokenType.KeywordReturn,
                "for" => TokenType.KeywordFor,
                "while" => TokenType.KeywordWhile,
                "in" => TokenType.KeywordIn,
                "this" => TokenType.KeywordThis,
                "class" => TokenType.KeywordClass,
                "func" => TokenType.KeywordFunc,
                "new" => TokenType.KeywordNew,
                "and" => TokenType.And,
                "or" => TokenType.Or,
                "not" => TokenType.Not,
                "field" => TokenType.KeywordField,
                "ctor" => TokenType.KeywordCtor,
                "true" => TokenType.Boolean,
                "false" => TokenType.Boolean,
                "interface" => TokenType.KeywordInterface,
                _ => TokenType.Identifier,
            };
            return new Token(text, tokenType, new Range(index - text.Length, index));
        }

        return Consume(1, TokenType.Unknown);
    }

    private static bool IsIdent(char ch) => char.IsLetterOrDigit(ch) || ch == '_';

    private static bool IsNewline(char ch) => ch is '\n' or '\r';

    private char Peek(int offset = 0, char defaultChar = '\0')
    {
        if (index + offset >= code.Length) return defaultChar;
        return code[index + offset];
    }

    private string Consume(int length)
    {
        var text = code.Substring(index, length);
        index += length;
        return text;
    }

    private Token Consume(int length, TokenType tokenType)
    {
        var text = Consume(length);
        return new Token(text, tokenType, new Range(index - text.Length, index));
    }
}
