using SpiralCompiler.Syntax;

namespace SpiralCompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tokens = Lexer.Lex(File.ReadAllText("testcode_tokens.txt"));

            var output = new List<string>();
            foreach (var token in tokens) {
                output.Add(token.Text);
            }
            File.WriteAllLines("testcode_out.txt", output);
        }
    }
}