namespace SpiralCompiler.Semantics;
public sealed class Scope
{
    public IDictionary<string, List<Symbol>> Symbols { get; } = new Dictionary<string, List<Symbol>>();
    public Scope? Parent { get; }

    public Scope(Scope? parent)
    {
        Parent = parent;
    }

    public void AddSymbol(Symbol symbol)
    {
        if (Symbols.ContainsKey(symbol.Name))
        {
            Symbols[symbol.Name].Add(symbol);
        }
        else
        {
            Symbols.Add(symbol.Name, new List<Symbol> { symbol });
        }
    }

    public List<Symbol> SearchSymbol(string name)
    {
        if (Symbols.TryGetValue(name, out var symbols))
        {
            return symbols;
        }
        if (Parent is not null)
        {
            return Parent.SearchSymbol(name);
        }
        throw new ArgumentException($"symbol with specified name {name} not found");
    }
}
