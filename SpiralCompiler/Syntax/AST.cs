namespace SpiralCompiler.Syntax
{
    public class AST
    {
    }

    public abstract record class Statement
    {

    }

    public abstract record class Expression
    {
        public record class Integer(int Value) : Expression;
        public record class Double(double Value) : Expression;
        public record class String(string Value) : Expression;
        public record class Boolean(bool Value) : Expression;
        public record class Binary(Expression Left, BinOp Op, Expression Right) : Expression;
        public record class UnaryPre(UnOpPre Op, Expression Right) : Expression;
        public record class UnaryPost(Expression Left, UnOpPost Op) : Expression;
        public record class Identifier(string Name) : Expression;
    }

    public enum BinOp
    {
        Assign,
        Add,
        Substract,
        Multiply,
        Divide,
        Equals,
        Less,
        Greater,
        LessEquals,
        GreaterEquals,
        Or,
        And
    }

    public enum UnOpPre
    {
        Minus,
        Not,
    }

    public enum UnOpPost
    {
        Increment,
        Decrement,
    }
}
