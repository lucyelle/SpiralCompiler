using System.Collections;
using SpiralCompiler.Semantics;

namespace SpiralCompiler.Syntax;

public readonly record struct PrintableList<T>(List<T> Elements) : IList<T>
{
    public T this[int index] { get => ((IList<T>)Elements)[index]; set => ((IList<T>)Elements)[index] = value; }

    public int Count => ((ICollection<T>)Elements).Count;

    public bool IsReadOnly => ((ICollection<T>)Elements).IsReadOnly;

    public void Add(T item) => ((ICollection<T>)Elements).Add(item);
    public void Clear() => ((ICollection<T>)Elements).Clear();
    public bool Contains(T item) => ((ICollection<T>)Elements).Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)Elements).CopyTo(array, arrayIndex);
    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Elements).GetEnumerator();
    public int IndexOf(T item) => ((IList<T>)Elements).IndexOf(item);
    public void Insert(int index, T item) => ((IList<T>)Elements).Insert(index, item);
    public bool Remove(T item) => ((ICollection<T>)Elements).Remove(item);
    public void RemoveAt(int index) => ((IList<T>)Elements).RemoveAt(index);
    public override string ToString() => string.Join(", ", Elements);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Elements).GetEnumerator();

    public static implicit operator PrintableList<T>(List<T> elements) => new PrintableList<T>(elements);
}

public abstract record class Statement
{
    public sealed record class Var(string Name, TypeReference? Type, Expression? Value) : Statement
    {
        public Symbol? Symbol { get; set; }
    }
    public sealed record class If(Expression Condition, Statement Then, Statement? Else) : Statement;
    public sealed record class Block(PrintableList<Statement> Statements) : Statement
    {
        public Scope? Scope { get; set; }
    }
    public sealed record class FunctionDef(Visibility Visibility, string Name, PrintableList<Parameter> Params, TypeReference? ReturnType, Statement Body) : Statement
    {
        public Scope? Scope { get; set; }
        public Symbol.Function? Symbol { get; set; }
    }
    public sealed record class While(Expression Condition, Statement Body) : Statement;
    public sealed record class For(string Iterator, Expression Range, Statement Body) : Statement;
    public sealed record class Expr(Expression Expression) : Statement;
    public sealed record class Return(Expression? Expression) : Statement;
    public sealed record class Field(Visibility Visibility, string Name, TypeReference Type, Expression? Value) : Statement
    {
        public Symbol? Symbol { get; set; }
    }
    // TODO: ctor
    // TODO: diamond
    public sealed record class Class(Visibility Visibility, string Name, PrintableList<Field> Fields, PrintableList<FunctionDef> Functions, string? Base) : Statement
    {
        public Scope? Scope { get; set; }
        public Symbol.Type.Class? Symbol { get; set; }
    }
    public sealed record class Interface(Visibility Visibility, string Name, PrintableList<FunctionDef> Functions, string Base) : Statement
    {
        public Scope? Scope { get; set; }
        public Symbol.Type.Class? Symbol { get; set; }
    }
}

public abstract record class Expression
{
    public sealed record class Integer(int Value) : Expression;
    public sealed record class Double(double Value) : Expression;
    public sealed record class String(string Value) : Expression;
    public sealed record class Boolean(bool Value) : Expression;
    public sealed record class Binary(Expression Left, BinOp Op, Expression Right) : Expression;
    public sealed record class UnaryPre(UnOpPre Op, Expression Right) : Expression;
    public sealed record class UnaryPost(Expression Left, UnOpPost Op) : Expression;
    public sealed record class Identifier(string Name) : Expression
    {
        public Symbol? Symbol { get; set; }
    }
    public sealed record class FunctionCall(Expression Function, PrintableList<Expression> Args) : Expression
    {
        public List<Symbol>? Symbols { get; set; }
    }
    public sealed record class MemberAccess(Expression Left, string MemberName) : Expression
    {
        // Member symbol
        public Symbol? Symbol { get; set; }
    }
    // TODO: ctor
    public sealed record class New(string ClassName) : Expression;
}

public abstract record class TypeReference
{
    public sealed record class Identifier(string Name) : TypeReference
    {
        public Symbol? Symbol { get; set; }
    }
}

public sealed record class Parameter(string Name, TypeReference Type)
{
    public Symbol.Variable? Symbol { get; set; }
}

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
    Modulo,
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

public enum Visibility
{
    Public,
    Private,
}
