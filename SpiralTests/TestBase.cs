using System.Text;
using System.Xml.Linq;
using SpiralCompiler;
using SpiralCompiler.Syntax;
using SpiralCompiler.VM;

namespace SpiralTests;
public abstract class TestBase
{
    public static ByteCode Compile(string source)
    {
        var tokens = Lexer.Lex(source);
        var tree = Parser.Parse(tokens);
        var compilation = new Compilation(tree);
        return CodeGenerator.Generate(compilation.RootModule);
    }

    public static dynamic? CallFunction(ByteCode byteCode, string name, params object?[] args)
    {
        var addr = byteCode.GetAddress(name);
        var vm = new VirtualMachine(byteCode);
        return vm.Call(addr, args);
    }

    public static object? RunWithStdIO(ByteCode module, TextReader stdIn, TextWriter stdOut)
    {
        var oldIn = Console.In;
        var oldOut = Console.Out;

        Console.SetIn(stdIn);
        Console.SetOut(stdOut);

        var addr = module.GetAddress("main");
        var vm = new VirtualMachine(module);
        var result = vm.Call(addr);

        Console.SetIn(oldIn);
        Console.SetOut(oldOut);

        return result;
    }

    public static string RunAndCaptureStdOut(ByteCode byteCode, string input = "")
    {
        var sr = new StringReader(input);
        var sw = new StringWriter();
        RunWithStdIO(byteCode, sr, sw);
        return sw.ToString();
    }

    public static string TrimTrailingWhitespaces(string text)
    {
        var result = new StringBuilder();
        var sr = new StringReader(text);
        string? line = null;
        while ((line = sr.ReadLine()) is not null)
        {
            result.AppendLine(line.TrimEnd());
        }
        return result.ToString();
    }
}
