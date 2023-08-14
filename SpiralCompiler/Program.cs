using SpiralCompiler.Syntax;

namespace SpiralCompiler;

public class Program
{
    public static void Main(string[] args)
    {
        var text = File.ReadAllText("testcode_ast.txt");
        var tokens = Lexer.Lex(text);
        var tree = Parser.Parse(tokens);
        Console.WriteLine(tree);
    }
}
