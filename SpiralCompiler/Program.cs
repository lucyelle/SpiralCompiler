using SpiralCompiler.IR;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;
using SpiralCompiler.VM;

namespace SpiralCompiler;

public class Program
{
    public static void Main(string[] args)
    {
        var text = File.ReadAllText("D:\\EGYETEM\\VIK\\felev06\\önlab\\test\\testcode_ast.txt");
        var tokens = Lexer.Lex(text);
        var tree = Parser.Parse(tokens);
        var compilation = new Compilation(tree);
        var assembly = CodeGenerator.Generate(compilation.RootModule);
        var bytecode = ByteCodeCompiler.Compile(assembly);
        var vm = new VirtualMachine(bytecode);
        vm.Run();
        Console.WriteLine(bytecode);
    }
}
