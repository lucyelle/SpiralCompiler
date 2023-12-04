using System.CommandLine;
using System.IO.Pipelines;
using System.Reflection;
using Draco.Lsp.Server;

namespace SpiralLangserver;

internal enum TransportKind
{
    Unknown = 0,
    Stdio = 1,
    Ipc = 2,
    Pipe = 3,
    Socket = 4,
}

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        var stdioFlag = new Option<bool>(name: "--stdio", description: "A flag to set the transportation option to stdio");

        var runCommand = new Command("run", "Runs the language server")
        {
            stdioFlag,
        };
        runCommand.SetHandler(RunServerAsync, stdioFlag);

        var rootCommand = new RootCommand("Language Server for Spiral");
        rootCommand.AddCommand(runCommand);

        await rootCommand.InvokeAsync(args);
    }

    internal static async Task RunServerAsync(bool stdioFlag)
    {
        var transportKind = GetTransportKind(stdioFlag);
        var transportStream = BuildTransportStream(transportKind);

        var client = LanguageServer.Connect(transportStream);
        var server = new SpiralLanguageServer(client);
        await client.RunAsync(server);
    }

    private static TransportKind GetTransportKind(bool stdioFlag) => stdioFlag
        ? TransportKind.Stdio
        : TransportKind.Unknown;

    private static IDuplexPipe BuildTransportStream(TransportKind transportKind)
    {
        if (transportKind == TransportKind.Stdio)
        {
            return new StdioDuplexPipe();
        }

        Console.Error.WriteLine($"The transport kind {transportKind} is not yet supported");
        Environment.Exit(1);
        return null;
    }

    private sealed class StdioDuplexPipe : IDuplexPipe
    {
        public PipeReader Input { get; } = PipeReader.Create(Console.OpenStandardInput());

        public PipeWriter Output { get; } = PipeWriter.Create(Console.OpenStandardOutput());
    }
}
