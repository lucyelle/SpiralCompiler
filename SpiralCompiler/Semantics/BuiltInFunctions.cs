namespace SpiralCompiler.Semantics;
public static class BuiltInFunctions
{
    public static Dictionary<Symbol.Function, Delegate> Delegates { get; } = new();

    // TODO: universal print, printLine
    static BuiltInFunctions()
    {
        Create("printLine", new List<Symbol.Type>() { BuiltInTypes.String }, BuiltInTypes.Void, (string value) => Console.WriteLine(value));
        Create("printLine", new List<Symbol.Type>() { BuiltInTypes.Int }, BuiltInTypes.Void, (int value) => Console.WriteLine(value));
        Create("printLine", new List<Symbol.Type>() { BuiltInTypes.Double }, BuiltInTypes.Void, (double value) => Console.WriteLine(value));
        Create("print", new List<Symbol.Type>() { BuiltInTypes.String }, BuiltInTypes.Void, (string value) => Console.Write(value));
        Create("print", new List<Symbol.Type>() { BuiltInTypes.Int }, BuiltInTypes.Void, (int value) => Console.Write(value));
        Create("print", new List<Symbol.Type>() { BuiltInTypes.Double }, BuiltInTypes.Void, (double value) => Console.Write(value));
    }

    public static Symbol.Function Create(string name, List<Symbol.Type> parameters, Symbol.Type returnType, Delegate func)
    {
        var variables = parameters.Select(p => new Symbol.Variable("") { SymbolType = p }).ToList();
        var function = new Symbol.Function(name, variables)
        {
            ReturnType = returnType,
            SymbolType = new Symbol.Type.Function(parameters, returnType),
        };

        Delegates.Add(function, func);

        return function;
    }
}
