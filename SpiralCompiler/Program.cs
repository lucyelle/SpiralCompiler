using SpiralCompiler.Syntax;

namespace SpiralCompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tokens = Lexer.Lex(File.ReadAllText("testcode_tokens.txt"));

            using var output = new FileStream("tokens_out.txt", FileMode.OpenOrCreate, FileAccess.Write);
            using var writer = new StreamWriter(output);
            foreach (var t in tokens)
            {
                writer.WriteLine($"'{t.Text}' => {t.Type} {t.Position.Start}-{t.Position.End}");
            }
        }
    }
}