import * as vscode from "vscode";

export async function activate(context: vscode.ExtensionContext) {
    vscode.window.showInformationMessage("Hello World from vscode-extensions!");
}

export async function deactivate(): Promise<void> {
}
