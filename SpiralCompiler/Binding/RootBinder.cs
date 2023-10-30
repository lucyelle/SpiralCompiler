using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;

namespace SpiralCompiler.Binding;

public sealed class RootBinder : Binder
{
    public override Binder? Parent => null;

    public override IEnumerable<Symbol> DeclaredSymbols { get; } = ImmutableArray.Create<Symbol>(
        BuiltInTypeSymbol.Int,
        OpCodeFunctionSymbol.Add_Int,
        OpCodeFunctionSymbol.Sub_Int,
        OpCodeFunctionSymbol.Mul_Int,
        OpCodeFunctionSymbol.Less_Int,
        OpCodeFunctionSymbol.Eq_String,
        OpCodeFunctionSymbol.PreIncrement_Int,
        OpCodeFunctionSymbol.Print_Int,
        OpCodeFunctionSymbol.Println_Int,
        OpCodeFunctionSymbol.Println_String,
        OpCodeFunctionSymbol.Readln);
}
