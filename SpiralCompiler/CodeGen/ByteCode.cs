using SpiralCompiler.Semantics;

namespace SpiralCompiler.CodeGen;

public abstract record class Operand
{
    public sealed record class Constant(object Value) : Operand;
    public sealed record class Register(int Index) : Operand;
    public sealed record class Local(Symbol Symbol) : Operand;
    public sealed record class Function(FunctionDef FuncDef) : Operand;
}

public sealed record class Label(string Name);

public sealed record class FunctionDef(List<Operand.Local> Params, List<Operand.Local> Locals, List<BasicBlock> Body);

public record class BasicBlock(Label Label, List<Instruction> Instructions);

public record class Instruction
{
    public sealed record class Call(Operand.Register Target, Operand Func, List<Operand> Params) : Instruction;
    public sealed record class Load(Operand.Register Target, Operand.Local Source) : Instruction;
    public sealed record class Store(Operand.Local Target, Operand Source) : Instruction;
    public sealed record class Goto(Label Label) : Instruction;
    public sealed record class GotoIf(Operand Condition, Label Then, Label Else) : Instruction;
    public sealed record class Arithmetic(Operand.Register Target, ArithmeticOp Op, Operand Left, Operand Right) : Instruction;
    public sealed record class Return(Operand? Value) : Instruction;
}

public enum ArithmeticOp
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Assign,
    Equals,
    Less,
    Greater,
}
