using SpiralCompiler.Syntax;

namespace SpiralCompiler;

public class Program
{
    static void Main(string[] args)
    {
        TokensTest("testcode_tokens.txt", "tokens_out.txt");
    }

    private static void TokensTest(string inputPath, string outputPath)
    {
        var tokens = Lexer.Lex(File.ReadAllText(inputPath));

        using var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);
        using var writer = new StreamWriter(output);
        foreach (var t in tokens)
        {
            writer.WriteLine($"'{t.Text}' => {t.Type} {t.Position.Start}-{t.Position.End}");
        }
    }
}