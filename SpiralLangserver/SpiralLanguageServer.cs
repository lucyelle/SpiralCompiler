using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Draco.Lsp.Model;
using Draco.Lsp.Server;
using Draco.Lsp.Server.Language;
using Draco.Lsp.Server.TextDocument;
using Microsoft.VisualBasic;
using SpiralCompiler;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;
using static System.Net.Mime.MediaTypeNames;

namespace SpiralLangserver;

internal sealed class SpiralLanguageServer : ILanguageServer, ITextDocumentSync, ICodeCompletion
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

    public CompletionRegistrationOptions CompletionRegistrationOptions => new()
    {
        DocumentSelector = DocumentSelector,
        TriggerCharacters = new[] { "." },
    };

    private readonly ILanguageClient client;
    private string? sourceText;

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

    public async Task<IList<CompletionItem>> CompleteAsync(CompletionParams param, CancellationToken cancellationToken)
    {
        if (sourceText is null)
        {
            return new List<CompletionItem>();
        }

        var lines = sourceText.Split('\n');
        var indexOfCursor = PositionToIndex(lines, param.Position);

        var tokens = Lexer.Lex(sourceText);
        var tree = Parser.Parse(tokens);
        var lastMemberAccess = tree.GetNodesIntersectingIndex(indexOfCursor)
            .OfType<MemberExpressionSyntax>()
            .LastOrDefault();
        if (lastMemberAccess is null)
        {
            return new List<CompletionItem>();
        }

        var compilation = new Compilation(tree);
        var typeOfLeft = compilation.TypeOf(lastMemberAccess.Left);

        return typeOfLeft.Members
            .Select(m => new CompletionItem()
            {
                Label = m.Name,
                Kind = m switch
                {
                    FieldSymbol => CompletionItemKind.Field,
                    FunctionSymbol f when f.IsConstructor => CompletionItemKind.Function,
                    FunctionSymbol => CompletionItemKind.Function,
                    _ => null,
                },
            })
            .ToList();
    }

    private async Task ReportErrors(DocumentUri uri, string contents)
    {
        contents = contents
            .Replace("\r\n", "\n")
            .Replace('\r', '\n');
        sourceText = contents;
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

    private static Position IndexToPosition(IEnumerable<string> lines, int index)
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

    private static int PositionToIndex(IEnumerable<string> lines, Position position)
    {
        var index = 0;
        var lineIndex = 0;
        foreach (var line in lines)
        {
            if (lineIndex == position.Line) break;
            index += line.Length + 1;
            lineIndex++;
        }
        return index + (int)position.Character;
    }
}
