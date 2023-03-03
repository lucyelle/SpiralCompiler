namespace SpiralCompiler.Syntax;

public readonly record struct PrintableList<T>(List<T> Elements)
{
    public override string ToString() => string.Join(", ", Elements);

    public static implicit operator PrintableList<T>(List<T> elements) => new PrintableList<T>(elements);
}

public abstract record class Statement
{
    public record class Var(string Name, TypeReference? Type, Expression? Value) : Statement;
    public record class If(Expression Condition, Statement Then, Statement? Else) : Statement;
    public record class Block(PrintableList<Statement> Statements) : Statement;
    public record class FunctionDef(string Name, PrintableList<Parameter> Params, TypeReference? ReturnType, Statement Body) : Statement;
    public record class While(Expression Condition, Statement Body) : Statement;
    public record class For(string Iterator, Expression Range, Statement Body) : Statement;
    public record class Expr(Expression Expression) : Statement;
    public record class Return(Expression? Expression) : Statement;
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
    public record class FunctionCall(Expression Function, PrintableList<Expression> Params) : Expression;
    // TODO: member access
}

public abstract record class TypeReference
{
    public record class Identifier(string Name) : TypeReference;
}

public sealed record class Parameter(string Name, TypeReference Type);

public enum BinOp
{
    Assign,
    Add,
    Substract,
    Multiply,
    Divide,
    Equals,
    NotEqual,
    Less,
    Greater,
    LessEquals,
    GreaterEquals,
    Or,
    And,
    AddAssign,
    SubtractAssign,
    MultiplyAssign,
    DivideAssign,
}

public enum UnOpPre
{
    Increment,
    Decrement,
    Plus,
    Minus,
    Not,
}

public enum UnOpPost
{
    Increment,
    Decrement,
}
