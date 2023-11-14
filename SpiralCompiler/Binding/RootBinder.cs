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
        BuiltInTypeSymbol.Bool,
        BuiltInTypeSymbol.String,
        OpCodeFunctionSymbol.Not_Bool,
        OpCodeFunctionSymbol.Neg_Int,
        OpCodeFunctionSymbol.Add_Int,
        OpCodeFunctionSymbol.Sub_Int,
        OpCodeFunctionSymbol.Mul_Int,
        OpCodeFunctionSymbol.Mod_Int,
        OpCodeFunctionSymbol.Div_Int,
        OpCodeFunctionSymbol.Less_Int,
        OpCodeFunctionSymbol.Greater_Int,
        OpCodeFunctionSymbol.LeEq_Int,
        OpCodeFunctionSymbol.Eq_Int,
        OpCodeFunctionSymbol.GrEq_Int,
        OpCodeFunctionSymbol.Add_String,
        OpCodeFunctionSymbol.Eq_String,
        OpCodeFunctionSymbol.Print_Int,
        OpCodeFunctionSymbol.Print_String,
        OpCodeFunctionSymbol.Println_Int,
        OpCodeFunctionSymbol.Println_String,
        OpCodeFunctionSymbol.Readln,
        OpCodeFunctionSymbol.Length,
        OpCodeFunctionSymbol.Isnumber,
        OpCodeFunctionSymbol.Stoi,
        OpCodeFunctionSymbol.Itos);
}
