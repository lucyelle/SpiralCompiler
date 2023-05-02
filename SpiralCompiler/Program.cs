using SpiralCompiler.CodeGen;
using SpiralCompiler.Semantics;
using SpiralCompiler.Syntax;

namespace SpiralCompiler;

public class Program
{
    public static void Main(string[] args)
    {
        TokensTest("test.txt");
    }

    private static void TokensTest(string inputPath)
    {
        var tokens = Lexer.Lex(File.ReadAllText(inputPath));

        var ast = Parser.Parse(tokens);
        SemanticsChecking.PassPipeline(ast);

        //Console.WriteLine(compiler);

        var module = Compiler.Compile(ast);

        var interpreter = new Interpreter(module);
        interpreter.Run("main", Array.Empty<object>());
        var fib = interpreter.Run("fib", new object[] { 5 });
        Console.WriteLine(fib);
    }
}
