namespace SpiralCompiler.CodeGen;
public class StackFrame
{
    public Dictionary<Operand.Register, object> Registers { get; set; } = new();
    public Dictionary<Operand.Local, object> Variables { get; set; } = new();
    public List<object>? Args { get; set; }

    // Where to set the IP once returned
    public int ReturnAddress { get; set; }

    // Where to store the returned value
    public Operand.Register? ReturnRegister;
}
