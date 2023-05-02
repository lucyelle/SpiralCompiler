using SpiralCompiler.Syntax;

namespace SpiralCompiler.Semantics;
public class SemanticsChecking
{
    public static void PassPipeline(Statement ast)
    {
        var symbolRes1 = new SymbolResolutionStage1();
        symbolRes1.VisitStatement(ast);
        var symbolRes2 = new SymbolResolutionStage2(symbolRes1.RootScope);
        symbolRes2.VisitStatement(ast);

        var typeChecker1 = new TypeCheckingStage1();
        typeChecker1.VisitStatement(ast);
        var typeChecker2 = new TypeCheckingStage2();
        typeChecker2.VisitStatement(ast);
    }
}
