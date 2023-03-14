namespace SpiralCompiler.Semantics;
public abstract record class Symbol(string Name)
{
    public sealed record class Variable(string Name) : Symbol(Name), ITyped
    {
        public Type? SymbolType { get; set; }
    }

    public sealed record class Function(string Name, List<Variable> Params) : Symbol(Name), ITyped
    {
        public Type? ReturnType { get; set; }
        public Type? SymbolType { get; set; }
    }

    public abstract record class Type(string Name) : Symbol(Name)
    {
        public sealed record class Primitive(string Name, System.Type UnderlyingType) : Type(Name);
        new public sealed record class Function(List<Type> ParamTypes, Type ReturnType) : Type("");
        // TODO: class
    }

    public interface ITyped
    {
        public Type? SymbolType { get; set; }
    }
}
