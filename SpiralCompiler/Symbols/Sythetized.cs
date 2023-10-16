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
    public static TypeSymbol Void { get; } = new BuiltInTypeSymbol("void");
    public static TypeSymbol Int { get; } = new BuiltInTypeSymbol("int");
    public static TypeSymbol Bool { get; } = new BuiltInTypeSymbol("bool");

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
    public static FunctionSymbol Print_Int { get; } = IntrinsicFunction(
        "print",
        new[] { BuiltInTypeSymbol.Int },
        BuiltInTypeSymbol.Void,
        args => { Console.Write(args[0]); return null!; });
    public static FunctionSymbol Println_Int { get; } = IntrinsicFunction(
        "println",
        new[] { BuiltInTypeSymbol.Int },
        BuiltInTypeSymbol.Void,
        args => { Console.WriteLine(args[0]); return null!; });

    public static FunctionSymbol Add_Int { get; } = BinaryNumericOperator(TokenType.Plus, BuiltInTypeSymbol.Int, OpCode.Add);
    public static FunctionSymbol Sub_Int { get; } = BinaryNumericOperator(TokenType.Minus, BuiltInTypeSymbol.Int, OpCode.Sub);
    public static FunctionSymbol Mul_Int { get; } = BinaryNumericOperator(TokenType.Multiply, BuiltInTypeSymbol.Int, OpCode.Mul);

    public static FunctionSymbol Less_Int { get; } = RelationalOperator(TokenType.LessThan, BuiltInTypeSymbol.Int, OpCode.Less);

    public static FunctionSymbol PreIncrement_Int { get; } = PrefixUnaryOperator(TokenType.Increment, BuiltInTypeSymbol.Int, BuiltInTypeSymbol.Int, new[]
    {
        Instr(OpCode.PushConst, 1),
        Instr(OpCode.Add),
    });

    public static OpCodeFunctionSymbol BinaryNumericOperator(TokenType op, TypeSymbol numberType, OpCode opCode) =>
        BinaryOperator(op, numberType, numberType, numberType, new[] { Instr(opCode) });

    public static OpCodeFunctionSymbol RelationalOperator(TokenType op, TypeSymbol numberType, OpCode opCode) =>
        BinaryOperator(op, numberType, numberType, BuiltInTypeSymbol.Bool, new[] { Instr(opCode) });

    public static OpCodeFunctionSymbol BinaryOperator(
        TokenType op,
        TypeSymbol leftType,
        TypeSymbol rightType,
        TypeSymbol resultType,
        Instruction[] instructions) => new(
            GetBinaryOperatorName(op),
            Params(leftType, rightType),
            resultType,
            instructions.ToImmutableArray());

    public static OpCodeFunctionSymbol PrefixUnaryOperator(
        TokenType op,
        TypeSymbol subexprType,
        TypeSymbol resultType,
        Instruction[] instructions) => new(
            GetPrefixUnaryOperatorName(op),
            Params(subexprType),
            resultType,
            instructions.ToImmutableArray());

    public static OpCodeFunctionSymbol IntrinsicFunction(
        string name,
        TypeSymbol[] paramTypes,
        TypeSymbol returnType,
        Func<dynamic?[], dynamic?> method) => new(
        name,
        paramTypes.Select(Param).ToImmutableArray(),
        returnType,
        ImmutableArray.Create(Instr(OpCode.CallInt, method, paramTypes.Length)));

    private static ImmutableArray<ParameterSymbol> Params(params TypeSymbol[] types) =>
        types.Select(Param).ToImmutableArray();
    private static ParameterSymbol Param(TypeSymbol type) => new SynthetizedParameterSymbol(type);
    private static Instruction Instr(OpCode opCode, params object?[] args) => new(opCode, args);

    public override string Name { get; }
    public override ImmutableArray<ParameterSymbol> Parameters { get; }
    public override TypeSymbol ReturnType { get; }
    public ImmutableArray<Instruction> Instructions { get; set; }

    public OpCodeFunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol returnType, ImmutableArray<Instruction> instructions)
    {
        Name = name;
        Parameters = parameters;
        ReturnType = returnType;
        Instructions = instructions;
    }
}

public sealed class SynthetizedDefaultConstructorSymbol : FunctionSymbol
{
    public override ImmutableArray<ParameterSymbol> Parameters => ImmutableArray<ParameterSymbol>.Empty;
    public override TypeSymbol ReturnType { get; }
    public override bool IsConstructor => true;

    public SynthetizedDefaultConstructorSymbol(TypeSymbol returnType)
    {
        ReturnType = returnType;
    }
}
