using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralCompiler.Symbols;
public sealed class BuiltInTypeSymbol : TypeSymbol
{
    public static TypeSymbol Int { get; } = new BuiltInTypeSymbol("int");

    public override string Name { get; }

    public BuiltInTypeSymbol(string name)
    {
        Name = name;
    }
}
