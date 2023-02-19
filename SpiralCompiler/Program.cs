using SpiralCompiler.Syntax;

namespace SpiralCompiler;

public class Program
{
    static void Main(string[] args)
    {
        TokensTest("testcode_ast.txt", "ast_out.txt");
    }

    private static void TokensTest(string inputPath, string outputPath)
    {
        var tokens = Lexer.Lex(File.ReadAllText(inputPath));

        var ast = Parser.Parse(tokens);
        Console.WriteLine(ast);
    }
}