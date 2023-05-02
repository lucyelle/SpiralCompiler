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
}
