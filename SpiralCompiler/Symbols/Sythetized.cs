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
    public static TypeSymbol Error { get; } = new BuiltInTypeSymbol("<error>");
    public static TypeSymbol Void { get; } = new BuiltInTypeSymbol("void");
    public static TypeSymbol Int { get; } = new BuiltInTypeSymbol("int");
    public static TypeSymbol Bool { get; } = new BuiltInTypeSymbol("bool");
    public static TypeSymbol String { get; } = new BuiltInTypeSymbol("string");

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
    public static FunctionSymbol Print_String { get; } = IntrinsicFunction(
        "print",
        new[] { BuiltInTypeSymbol.String },
        BuiltInTypeSymbol.Void,
        args => { Console.Write(args[0]); return null!; });
    public static FunctionSymbol Println_Int { get; } = IntrinsicFunction(
        "println",
        new[] { BuiltInTypeSymbol.Int },
        BuiltInTypeSymbol.Void,
        args => { Console.WriteLine(args[0]); return null!; });
    public static FunctionSymbol Println_String { get; } = IntrinsicFunction(
        "println",
        new[] { BuiltInTypeSymbol.String },
        BuiltInTypeSymbol.Void,
        args => { Console.WriteLine(args[0]); return null!; });
    public static FunctionSymbol Readln { get; } = IntrinsicFunction(
        "readln",
        Array.Empty<TypeSymbol>(),
        BuiltInTypeSymbol.String,
        args => { return Console.ReadLine(); });
    public static FunctionSymbol Length { get; } = IntrinsicFunction(
        "length",
        new[] { BuiltInTypeSymbol.String },
        BuiltInTypeSymbol.Int,
        args => { return args[0]!.Length; });
    public static FunctionSymbol Isnumber { get; } = IntrinsicFunction(
        "isnumber",
        new[] { BuiltInTypeSymbol.String },
        BuiltInTypeSymbol.Bool,
        args => { return ((string)args[0]!).All(char.IsDigit); });
    public static FunctionSymbol Stoi { get; } = IntrinsicFunction(
        "stoi",
        new[] { BuiltInTypeSymbol.String },
        BuiltInTypeSymbol.Int,
        args => { return int.Parse(args[0]); });
    public static FunctionSymbol Itos { get; } = IntrinsicFunction(
        "itos",
        new[] { BuiltInTypeSymbol.Int },
        BuiltInTypeSymbol.String,
        args => { return args[0]!.ToString(); });

    public static FunctionSymbol Neg_Int { get; } = PrefixUnaryOperator(TokenType.Minus, BuiltInTypeSymbol.Int, BuiltInTypeSymbol.Int, new[]
    {
        Instr(OpCode.PushConst, -1),
        Instr(OpCode.Mul),
    });
    public static FunctionSymbol Not_Bool { get; } = PrefixUnaryOperator(TokenType.Not, BuiltInTypeSymbol.Bool, BuiltInTypeSymbol.Bool, new[] { Instr(OpCode.Not) });

    public static FunctionSymbol Add_Int { get; } = BinaryNumericOperator(TokenType.Plus, BuiltInTypeSymbol.Int, OpCode.Add);
    public static FunctionSymbol Sub_Int { get; } = BinaryNumericOperator(TokenType.Minus, BuiltInTypeSymbol.Int, OpCode.Sub);
    public static FunctionSymbol Mul_Int { get; } = BinaryNumericOperator(TokenType.Multiply, BuiltInTypeSymbol.Int, OpCode.Mul);
    public static FunctionSymbol Mod_Int { get; } = BinaryNumericOperator(TokenType.Modulo, BuiltInTypeSymbol.Int, OpCode.Mod);
    public static FunctionSymbol Div_Int { get; } = BinaryNumericOperator(TokenType.Divide, BuiltInTypeSymbol.Int, OpCode.Div);

    public static FunctionSymbol Less_Int { get; } = RelationalOperator(TokenType.LessThan, BuiltInTypeSymbol.Int, OpCode.Less);
    public static FunctionSymbol Eq_Int { get; } = RelationalOperator(TokenType.Equals, BuiltInTypeSymbol.Int, OpCode.Equals);
    public static FunctionSymbol GrEq_Int { get; } = BinaryOperator(TokenType.GreaterEquals, BuiltInTypeSymbol.Int, BuiltInTypeSymbol.Int, BuiltInTypeSymbol.Bool, new[]
    {
        Instr(OpCode.Less),
        Instr(OpCode.Not),
    });
    public static FunctionSymbol Greater_Int { get; } = BinaryOperator(TokenType.GreaterThan, BuiltInTypeSymbol.Int, BuiltInTypeSymbol.Int, BuiltInTypeSymbol.Bool, new[]
    {
        Instr(OpCode.Swap),
        Instr(OpCode.Less),
    });
    public static FunctionSymbol LeEq_Int { get; } = BinaryOperator(TokenType.LessEquals, BuiltInTypeSymbol.Int, BuiltInTypeSymbol.Int, BuiltInTypeSymbol.Bool, new[]
    {
        Instr(OpCode.Swap),
        Instr(OpCode.Less),
        Instr(OpCode.Not),
    });

    public static FunctionSymbol Add_String { get; } = BinaryNumericOperator(TokenType.Plus, BuiltInTypeSymbol.String, OpCode.Add);
    public static FunctionSymbol Eq_String { get; } = RelationalOperator(TokenType.Equals, BuiltInTypeSymbol.String, OpCode.Equals);

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
    public override string Name => ".ctor";

    public SynthetizedDefaultConstructorSymbol(TypeSymbol returnType)
    {
        ReturnType = returnType;
    }
}

public sealed class SynthetizedGlobalInitializerSymbol : FunctionSymbol
{
    public override string Name => ".global_initializer";
    public override ImmutableArray<ParameterSymbol> Parameters => ImmutableArray<ParameterSymbol>.Empty;
    public override TypeSymbol ReturnType => BuiltInTypeSymbol.Void;

    public SynthetizedGlobalInitializerSymbol()
    {
    }
}
