using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draco.Lsp.Model;
using Draco.Lsp.Server;
using Draco.Lsp.Server.TextDocument;
using SpiralCompiler;
using SpiralCompiler.Syntax;
using static System.Net.Mime.MediaTypeNames;

namespace SpiralLangserver;

internal sealed class SpiralLanguageServer : ILanguageServer, ITextDocumentSync
{
    public InitializeResult.ServerInfoResult Info => new()
    {
        Name = "Spiral Language Server",
        Version = "0.1.0",
    };

    public IList<DocumentFilter> DocumentSelector => new[]
    {
        new DocumentFilter()
        {
            Language = "spiral",
            Pattern = "**/*.spiral",
        }
    };

    public TextDocumentSyncKind SyncKind => TextDocumentSyncKind.Full;

    private readonly ILanguageClient client;

    public SpiralLanguageServer(ILanguageClient client)
    {
        this.client = client;
    }

    public void Dispose() { }

    public Task InitializeAsync(InitializeParams param) => Task.CompletedTask;
    public Task InitializedAsync(InitializedParams param) => Task.CompletedTask;
    public Task ShutdownAsync() => Task.CompletedTask;

    public async Task TextDocumentDidOpenAsync(DidOpenTextDocumentParams param)
    {
        await ReportErrors(param.TextDocument.Uri, param.TextDocument.Text);
    }

    public async Task TextDocumentDidChangeAsync(DidChangeTextDocumentParams param)
    {
        await ReportErrors(param.TextDocument.Uri, param.ContentChanges[0].Text);
    }

    public Task TextDocumentDidCloseAsync(DidCloseTextDocumentParams param) => Task.CompletedTask;

    private async Task ReportErrors(DocumentUri uri, string contents)
    {
        contents = contents
            .Replace("\r\n", "\n")
            .Replace('\r', '\n');
        var errorMessages = GetErrorMessages(contents);
        var lines = contents.Split('\n');

        var diags = new List<Diagnostic>();

        foreach (var err in errorMessages)
        {
            diags.Add(new Diagnostic()
            {
                Message = err.Message,
                Range = err.Range is null
                    ? new Draco.Lsp.Model.Range()
                    {
                        Start = new Position() { Line = 0, Character = 0 },
                        End = new Position() { Line = 0, Character = 0 },
                    }
                    : new Draco.Lsp.Model.Range()
                    {
                        Start = IndexToPosition(lines, err.Range.Value.Start),
                        End = IndexToPosition(lines, err.Range.Value.End),
                    }
            });
        }

        await client.PublishDiagnosticsAsync(new PublishDiagnosticsParams()
        {
            Uri = uri,
            Diagnostics = diags,
        });
    }

    private static IReadOnlyList<ErrorMessage> GetErrorMessages(string contents)
    {
        var tokens = Lexer.Lex(contents);
        var tree = Parser.Parse(tokens);
        var compilation = new Compilation(tree);
        return compilation.GetErrors().ToList();
    }

    private Position IndexToPosition(IEnumerable<string> lines, int index)
    {
        var lineIndex = 0;
        foreach (var line in lines)
        {
            if (index < line.Length) break;
            index -= line.Length + 1;
            lineIndex++;
        }
        return new Position()
        {
            Line = (uint)lineIndex,
            Character = (uint)index,
        };
    }
}
