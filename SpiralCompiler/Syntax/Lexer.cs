using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SpiralCompiler.Syntax
{
    public class Lexer
    {
        private readonly string code;
        private int index;

        private bool IsEnd => index >= code.Length;

        public Lexer(string code)
        {
            this.code = code;
        }

        public static List<Token> Lex(string code)
        {
            var lexer = new Lexer(code);
            var tokenlist = new List<Token>();

            while (true)
            {
                var token = lexer.Next();
                tokenlist.Add(token);

                if (token.Type == TokenType.EndOfFile) break;
            }
            return tokenlist;
        }

        public Token Next()
        {
        begin:
            if (IsEnd) return new Token("", TokenType.EndOfFile, new Range(index, index));

            var ch = Peek();
            if (char.IsWhiteSpace(ch))
            {
                ++index;
                goto begin;
            }

            // Comment
            if (ch == '/' && Peek(1) == '/')
            {
                index += 2;
                while (Peek(defaultChar: '\r') != '\r' && Peek(defaultChar: '\n') != '\n')
                {
                    ++index;
                }
                goto begin;
            }

            // string
            if (ch == '"')
            {
                var offset = 1;
                while (Peek(offset, defaultChar: '"') != '"') ++offset;
                ++offset;
                return Consume(offset, TokenType.String);
            }

            // braces
            if (ch == '{') return Consume(1, TokenType.OpenBrace);
            if (ch == '}') return Consume(1, TokenType.CloseBrace);

            // parens
            if (ch == '(') return Consume(1, TokenType.OpenParen);
            if (ch == ')') return Consume(1, TokenType.CloseParen);

            // comma
            if (ch == ',') return Consume(1, TokenType.Comma);

            // semicolon
            if (ch == ';') return Consume(1, TokenType.Semicolon);

            // <
            if (ch == '<')
            {
                // <=
                if (Peek(1) == '=')
                {
                    return Consume(2, TokenType.LessEquals);
                }
                return Consume(1, TokenType.LessThan);
            }

            // >
            if (ch == '>')
            {
                // >=
                if (Peek(1) == '=')
                {
                    return Consume(2, TokenType.GreaterEquals);
                }
                return Consume(1, TokenType.GreaterThan);
            }

            // == and =
            if (ch == '=')
            {
                if (Peek(1) == '=') return Consume(2, TokenType.Equals);
                else return Consume(1, TokenType.Assign);
            }

            // +
            if (ch == '+')
            {
                // ++
                if (Peek(1) == '+')
                {
                    return Consume(2, TokenType.Increment);
                }
                return Consume(1, TokenType.Plus);
            }

            // -
            if (ch == '-')
            {
                if (Peek(1) == '-')
                {
                    return Consume(2, TokenType.Decrement);
                }
                return Consume(1, TokenType.Minus);
            }

            // *
            if (ch == '*') return Consume(1, TokenType.Multiply);

            // /
            if (ch == '/') return Consume(1, TokenType.Divide);

            // range
            if (ch == '.' && Peek(1) == '.')
            {
                return Consume(2, TokenType.Range);
            }

            // Number
            if (char.IsDigit(ch))
            {
                var offset = 1;
                while (char.IsDigit(Peek(offset))) ++offset;

                // double
                if (Peek(offset) == '.' && char.IsDigit(Peek(offset + 1)))
                {
                    offset++;
                    while (char.IsDigit(Peek(offset))) ++offset;
                    return Consume(offset, TokenType.Double);
                }
                // integer
                return Consume(offset, TokenType.Integer);
            }

            // dot
            if (ch == '.') return Consume(1, TokenType.Dot);

            // Identifier-like
            if (IsIdent(ch))
            {
                var offset = 1;
                while (IsIdent(Peek(offset))) ++offset;
                var text = Consume(offset);

                var tokenType = text switch
                {
                    // TODO
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
                    _ => TokenType.Identifier,
                };
                return new Token(text, tokenType, new Range(index - text.Length, index));
            }

            return Consume(1, TokenType.Unknown);
        }

        private static bool IsIdent(char ch) => char.IsLetterOrDigit(ch) || ch == '_';

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
}
