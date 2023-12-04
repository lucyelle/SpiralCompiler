"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.deactivate = exports.activate = void 0;
const vscode = require("vscode");
async function activate(context) {
    vscode.window.showInformationMessage("Hello World from vscode-extensions!");
}
exports.activate = activate;
async function deactivate() {
}
exports.deactivate = deactivate;
//# sourceMappingURL=extension.js.map