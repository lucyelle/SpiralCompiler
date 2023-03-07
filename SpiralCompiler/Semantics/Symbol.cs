namespace SpiralCompiler.Semantics;
public abstract record class Symbol(string Name)
{
    public sealed record class Variable(string Name) : Symbol(Name);
    public abstract record class Type(string Name) : Symbol(Name)
    {
        public sealed record class Primitive(string Name, System.Type UnderlyingType) : Type(Name);
        // TODO: class
    }
    public sealed record class Function(string Name) : Symbol(Name);
}
