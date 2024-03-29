using System.Collections.Generic;
using System.Linq;

namespace SpiralCompiler.Syntax;

/// <summary>
/// 
/// </summary>
/// <param name="Start">Position of the first character of the Token it represents (inclusive)</param>
/// <param name="End">Position of the last character of the Token (exclusive)</param>
public readonly record struct Range(int Start, int End)
{
    public bool Contains(int position) => position >= Start && position < End;
}

public enum TokenType
{
    Range,          // ..
    KeywordIf,      // if
    KeywordElse,    // else
    KeywordWhile,   // while
    KeywordFor,     // for
    KeywordIn,      // in
    KeywordVar,     // var
    KeywordFunc,    // func
    KeywordClass,   // class
    KeywordThis,    // this
    KeywordReturn,  // return
    KeywordNew,     // new
    KeywordField,   // field
    KeywordCtor,    // ctor
    KeywordInterface,
    Dot,            // .
    Identifier,
    Integer,        // 123
    Double,         // 1.23
    String,         // "123"
    BraceOpen,      // {
    BraceClose,     // }
    ParenOpen,      // (
    ParenClose,     // )
    BracketOpen,    // [
    BracketClose,   // ]
    Comma,          // ,
    Semicolon,      // ;
    Colon,          // :
    LessThan,       // <
    LessEquals,     // <=
    GreaterThan,    // >
    GreaterEquals,  // >=
    Equals,         // ==
    NotEqual,       // !=
    Assign,         // =
    Plus,           // +
    Minus,          // -
    Multiply,       // *
    Divide,         // /
    MultiplyAssign, // *=
    DivideAssign,   // /=
    And,            // and
    Or,             // or
    Not,            // not
    AddAssign,      // +=
    SubtractAssign, // -=
    Boolean,        // true/false
    Modulo,         // %
    EndOfFile,
    Unknown,
}

public sealed record class Token(string Text, TokenType Type, Range Range) : SyntaxNode
{
    public override IEnumerable<SyntaxNode> Children => Enumerable.Empty<SyntaxNode>();

    public override string ToString() => Text;

    public override Range? GetRange() => Range;
}
