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
        var stage1 = new SymbolResolutionStage1();
        stage1.VisitStatement(ast);
        var stage2 = new SymbolResolutionStage2(stage1.RootScope);
        stage2.VisitStatement(ast);

        var typeChecker1 = new TypeCheckingStage1();
        typeChecker1.VisitStatement(ast);
        var typeChecker2 = new TypeCheckingStage2();
        typeChecker2.VisitStatement(ast);

        //Console.WriteLine(compiler);

        var interpreter = new Interpreter();
        interpreter.Run(ast);
    }
}
