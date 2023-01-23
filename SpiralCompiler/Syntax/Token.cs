using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.Syntax
{
    public record struct Range(int Start, int End);

    public enum TokenType
    {
        KeywordIf,      // if
        KeywordElse,    // else
        KeywordWhile,   // while
        KeywordFor,     // for
        KeywordIn,      // in
        Range,          // ..
        KeywordVar,     // var
        KeywordFunc,    // func
        KeywordClass,   // class
        KeywordThis,    // this
        KeywordReturn,  // return
        KeywordNew,     // new
        Dot,            // .
        Identifier,     
        Integer,        // 123
        String,         // "123"
        OpenBrace,      // {
        CloseBrace,     // }
        OpenParen,      // (
        CloseParen,     // )
        Comma,          // ,
        Semicolon,      // ;
        LessThan,       // <
        LessEquals,     // <=
        GreaterThan,    // >
        GreaterEquals,  // >=
        Equals,         // ==
        Assign,         // =
        Plus,           // +
        Minus,          // -
        Multiply,       // *
        Divide,         // /
        And,            // and
        Or,             // or
        Increment,      // ++
        Decrement,      // --
        EndOfFile,
    }

    public record class Token(string Text, TokenType Type, Range Position);
}
