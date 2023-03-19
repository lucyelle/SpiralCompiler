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

        public static bool IsAssignable(Type left, Type right)
        {
            if (left == right)
            {
                return true;
            }
            else if (left == BuiltInTypes.Double && right == BuiltInTypes.Int)
            {
                return true;
            }
            return false;
        }

        public static bool IsNumeric(Type type) => type == BuiltInTypes.Int || type == BuiltInTypes.Double;

        public static Type CommonType(Type type1, Type type2)
        {
            if (type1 == type2)
            {
                return type1;
            }
            else if (IsNumeric(type1) && IsNumeric(type2))
            {
                if (type1 == BuiltInTypes.Double || type2 == BuiltInTypes.Double)
                {
                    return BuiltInTypes.Double;
                }
                return BuiltInTypes.Int;
            }
            else
            {
                throw new InvalidOperationException("no common type");
            }
        }
        // TODO: class
    }

    public interface ITyped
    {
        public Type? SymbolType { get; set; }
    }
}
