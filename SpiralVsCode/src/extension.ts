import * as vscode from "vscode";
import * as lsp from "vscode-languageclient/node";

export async function activate(context: vscode.ExtensionContext) {
    await startLanguageServer();
}

export async function deactivate(): Promise<void> {
}

async function startLanguageServer() {
    // Server options
    const serverOptions: lsp.ServerOptions = {
        command: 'spiral-langserver run',
        transport: lsp.TransportKind.stdio,
        options: {
            shell: true,
        },
    };

    // Client options
    const clientOptions: lsp.LanguageClientOptions = {
        documentSelector: [{ scheme: 'file', language: 'spiral' }],
    };

    const languageClient = new lsp.LanguageClient(
        "spiralLanguageServer",
        'Spiral Language Server',
        serverOptions,
        clientOptions,
    );

    // Start the client, which also starts the server
    await languageClient.start();
}
