"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.deactivate = exports.activate = void 0;
const vscode = require("vscode");
const lsp = require("vscode-languageclient/node");
async function activate(context) {
    await startLanguageServer();
    vscode.window.showInformationMessage("Hello World from vscode-extensions!");
}
exports.activate = activate;
async function deactivate() {
}
exports.deactivate = deactivate;
async function startLanguageServer() {
    // Server options
    const serverOptions = {
        command: 'spiral-langserver run',
        transport: lsp.TransportKind.stdio,
        options: {
            shell: true,
        },
    };
    // Client options
    const clientOptions = {
        documentSelector: [{ scheme: 'file', language: 'spiral' }],
    };
    const languageClient = new lsp.LanguageClient("spiralLanguageServer", 'Spiral Language Server', serverOptions, clientOptions);
    // Start the client, which also starts the server
    await languageClient.start();
}
//# sourceMappingURL=extension.js.map