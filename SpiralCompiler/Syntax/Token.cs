namespace SpiralCompiler.Syntax;

/*
 * Start: position of the first character of the Token it represents (inclusive)
 * End: position of the last character of the Token (exclusive)
*/
public readonly record struct Range(int Start, int End);

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
    Double,         // 1.23
    String,         // "123"
    BraceOpen,      // {
    BlaceClose,     // }
    ParenOpen,      // (
    ParenClose,     // )
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
    Unknown,
}

public sealed record class Token(string Text, TokenType Type, Range Position);
