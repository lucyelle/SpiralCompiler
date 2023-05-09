using System.Text;
using SpiralCompiler.CodeGen;
using SpiralCompiler.Semantics;
using SpiralCompiler.Syntax;

namespace SpiralTests;
public abstract class TestBase
{
    public static Module Compile(string source)
    {
        var tokens = Lexer.Lex(source);

        var ast = Parser.Parse(tokens);
        SemanticsChecking.PassPipeline(ast);

        var module = Compiler.Compile(ast);

        return module;
    }

    public static object? CallFunction(Module module, string name, params object?[] args)
    {
        var interpreter = new Interpreter(module);
        return interpreter.Run(name, args);
    }

    public static object? RunWithStdIO(Module module, TextReader stdIn, TextWriter stdOut)
    {
        var oldIn = Console.In;
        var oldOut = Console.Out;

        Console.SetIn(stdIn);
        Console.SetOut(stdOut);

        var interpreter = new Interpreter(module);
        var result = interpreter.Run("main", Array.Empty<object>());

        Console.SetIn(oldIn);
        Console.SetOut(oldOut);

        return result;
    }

    public static string RunAndCaptureStdOut(Module module, string input = "")
    {
        var sr = new StringReader(input);
        var sw = new StringWriter();
        RunWithStdIO(module, sr, sw);
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
