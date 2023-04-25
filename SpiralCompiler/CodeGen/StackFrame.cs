namespace SpiralCompiler.CodeGen;
public sealed class StackFrame
{
    public Dictionary<Operand.Register, object?> Registers { get; set; } = new();
    public Dictionary<Operand.Local, object?> Variables { get; set; } = new();

    // Where to set the IP once returned
    public int ReturnAddress { get; init; }

    // Where to store the returned value
    public Operand.Register? ReturnRegister { get; init; }
}
