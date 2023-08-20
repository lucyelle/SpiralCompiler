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
        var compilation = new Compilation(tree);

        foreach (var member in compilation.RootModule.Members)
        {
            if (member is SourceFunctionSymbol sourceFunction)
            {
                Console.WriteLine(sourceFunction.Body);
            }
            Console.WriteLine(member.Name);
        }
    }
}
