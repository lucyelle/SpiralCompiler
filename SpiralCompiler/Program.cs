using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler;

public class Program
{
    public static void Main(string[] args)
    {
        var text = File.ReadAllText("testcode_ast.txt");
        var tokens = Lexer.Lex(text);
        var tree = Parser.Parse(tokens);
        var module = new SourceModuleSymbol(tree);

        foreach (var member in module.Members)
        {
            Console.WriteLine(member.Name);
        }
    }
}
