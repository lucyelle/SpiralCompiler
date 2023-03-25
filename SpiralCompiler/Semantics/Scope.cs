namespace SpiralCompiler.Semantics;
public sealed class Scope
{
    public IDictionary<string, Symbol> Symbols { get; } = new Dictionary<string, Symbol>();
    public Scope? Parent { get; }

    public Scope(Scope? parent)
    {
        Parent = parent;
    }

    public void AddSymbol(Symbol symbol)
    {
        Symbols.Add(symbol.Name, symbol);
    }

    public Symbol SearchSymbol(string name)
    {
        if (Symbols.TryGetValue(name, out var symbol))
        {
            return symbol;
        }
        if (Parent is not null)
        {
            return Parent.SearchSymbol(name);
        }
        throw new ArgumentException($"symbol with specified name {name} not found");
    }
}
