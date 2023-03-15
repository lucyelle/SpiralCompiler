namespace SpiralCompiler.Semantics;
public static class BuiltInTypes
{
    public static Symbol.Type String { get; } = new Symbol.Type.Primitive("string", typeof(string));
    public static Symbol.Type Int { get; } = new Symbol.Type.Primitive("int", typeof(int));
    public static Symbol.Type Void { get; } = new Symbol.Type.Primitive("void", typeof(void));
}
