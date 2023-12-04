using System;
using System.IO;
using System.Linq;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;
using SpiralCompiler.VM;

namespace SpiralCompiler;

public class Program
{
    public static void Main(string[] args)
    {
        var text = File.ReadAllText(@"C:\TMP\spiral_input.spiral");
        var tokens = Lexer.Lex(text);
        var tree = Parser.Parse(tokens);
        var compilation = new Compilation(tree);

        var errors = compilation.GetErrors();
        if (errors.Any())
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
            return;
        }

        var bytecode = CodeGenerator.Generate(compilation.RootModule);
        Console.WriteLine(bytecode);
        var vm = new VirtualMachine(bytecode);
        var result = vm.Call(0);
        Console.WriteLine($"Returned: {result}");
    }
}
