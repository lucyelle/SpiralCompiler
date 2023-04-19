using SpiralCompiler.Semantics;

namespace SpiralCompiler.CodeGen;

public abstract record class Operand
{
    public sealed record class Constant(object Value) : Operand
    {
        public override string ToString() => Value.ToString()!;
    }
    public sealed record class Register(int Index) : Operand
    {
        public override string ToString() => $"r{Index}";
    }
    public sealed record class Local(Symbol Symbol) : Operand
    {
        public override string ToString() => Symbol.Name;
    }
    public sealed record class Function(FunctionDef FuncDef) : Operand
    {
        public override string ToString() => FuncDef.Symbol.Name;
    }
}

public sealed record class Label(string Name)
{
    public int Address { get; set; }

    public override string ToString() => $"{Name}:\n";
}

// TODO
public sealed record class Module(List<FunctionDef> FuncDefs)
{
    public override string ToString() => string.Join('\n', FuncDefs);
}

public sealed record class FunctionDef(Symbol.Function Symbol, List<Operand.Local> Params, List<Operand.Local> Locals, List<BasicBlock> Body)
{
    public int Address { get; set; }

    public override string ToString()
    {
        var parameters = string.Join(", ", Params.Select(p => p.Symbol.Name));

        var body = "";
        foreach (var b in Body)
        {
            body += b.ToString();
        }

        return $"func {Symbol.Name}({parameters}) : {Symbol.ReturnType} {{\n{body}}}\n";
    }
}

public record class BasicBlock(Label Label, List<Instruction> Instructions)
{
    public override string ToString()
    {
        var label = Label.ToString();
        var instructions = "";
        foreach (var i in Instructions)
        {
            instructions += i.ToString();
        }

        return $"{label}{instructions}";
    }
}

public record class Instruction
{
    public sealed record class Call(Operand.Register Target, Operand Func, List<Operand> Args) : Instruction
    {
        public int Address { get; set; }

        public override string ToString()
        {
            var reg = Target.ToString();
            var func = Func.ToString();
            var args = string.Join(", ", Args);

            return $"{reg} := {func}({args})\n";
        }
    }
    public sealed record class Load(Operand.Register Target, Operand.Local Source) : Instruction
    {
        public override string ToString()
        {
            var reg = Target.ToString();
            var local = Source.ToString();
            return $"{reg} := load {local}\n";
        }
    }
    public sealed record class Store(Operand.Local Target, Operand Source) : Instruction
    {
        public override string ToString()
        {
            var local = Target.ToString();
            var value = Source.ToString();
            return $"store {local}, {value}\n";
        }
    }
    public sealed record class Goto(Label Label) : Instruction
    {
        public override string ToString()
        {
            var label = Label.ToString();
            return $"goto {label}\n";
        }
    }
    public sealed record class GotoIf(Operand Condition, Label Then, Label Else) : Instruction
    {
        public override string ToString()
        {
            var condition = Condition.ToString();
            var thenLabel = Then.Name;
            var elseLabel = Else.Name;
            return $"if {condition} goto {thenLabel} else {elseLabel}\n";
        }
    }
    public sealed record class Arithmetic(Operand.Register Target, ArithmeticOp Op, Operand Left, Operand Right) : Instruction
    {
        public override string ToString()
        {
            var op = Op switch
            {
                ArithmeticOp.Add => "+",
                ArithmeticOp.Subtract => "-",
                ArithmeticOp.Multiply => "*",
                ArithmeticOp.Divide => "/",
                ArithmeticOp.Assign => "=",
                ArithmeticOp.Equals => "==",
                ArithmeticOp.Less => "<",
                ArithmeticOp.Greater => ">",
                _ => throw new NotImplementedException()
            };

            return $"{Target} := {Left} {op} {Right}\n";
        }
    }
    public sealed record class Return(Operand? Value) : Instruction
    {
        public override string ToString() => $"return {Value}\n";
    }
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
