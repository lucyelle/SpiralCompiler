using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Binding;

public sealed class BlockBinder : Binder
{
    public override Binder Parent { get; }

    public override IEnumerable<Symbol> DeclaredSymbols => declaredSymbols ??= BuildDeclaredSymbols();

    private ImmutableArray<Symbol>? declaredSymbols;

    private readonly BlockStatementSyntax blockStatement;

    public BlockBinder(Binder parent, BlockStatementSyntax blockStatement)
    {
        Parent = parent;
        this.blockStatement = blockStatement;
    }

    private ImmutableArray<Symbol> BuildDeclaredSymbols() => blockStatement.Statements
        .OfType<VariableDeclarationSyntax>()
        .Select(BuildLocalVariable)
        .ToImmutableArray();

    private Symbol BuildLocalVariable(VariableDeclarationSyntax syntax) => new SourceLocalVariableSymbol(syntax);
}
