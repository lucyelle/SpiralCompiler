using SpiralCompiler.Symbols;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpiralCompiler.Binding;

public sealed class ClassBinder : Binder
{
    public override Binder Parent { get; }

    public override IEnumerable<Symbol> DeclaredSymbols => @class.Members;

    private readonly SourceClassSymbol @class;

    public ClassBinder(Binder parent, SourceClassSymbol @class)
    {
        Parent = parent;
        this.@class = @class;
    }
}
