using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Syntax;
using SpiralCompiler.VM;

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

public sealed class SynthetizedParameterSymbol : ParameterSymbol
{
    public override TypeSymbol Type { get; }

    public SynthetizedParameterSymbol(TypeSymbol type)
    {
        Type = type;
    }
}

public sealed class OpCodeFunctionSymbol : FunctionSymbol
{
    public static FunctionSymbol Add_Int { get; } = BinaryNumericOperator(TokenType.Plus, BuiltInTypeSymbol.Int, OpCode.Add);

    public static OpCodeFunctionSymbol BinaryNumericOperator(TokenType op, TypeSymbol numberType, OpCode opCode) =>
        BinaryOperator(op, numberType, numberType, numberType, new[] { opCode });

    public static OpCodeFunctionSymbol BinaryOperator(
        TokenType op,
        TypeSymbol leftType,
        TypeSymbol rightType,
        TypeSymbol resultType,
        OpCode[] opCode) => new(
            GetOperatorName(op),
            ImmutableArray.Create<ParameterSymbol>(new SynthetizedParameterSymbol(leftType), new SynthetizedParameterSymbol(rightType)),
            resultType,
            opCode.ToImmutableArray());

    public override string Name { get; }
    public override ImmutableArray<ParameterSymbol> Parameters { get; }
    public override TypeSymbol ReturnType { get; }
    public ImmutableArray<OpCode> OpCodes { get; set; }

    public OpCodeFunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol returnType, ImmutableArray<OpCode> opCodes)
    {
        Name = name;
        Parameters = parameters;
        ReturnType = returnType;
        OpCodes = opCodes;
    }
}
