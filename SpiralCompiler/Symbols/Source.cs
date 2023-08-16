using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Symbols;

public sealed class SourceModuleSymbol : ModuleSymbol
{
    private readonly ProgramSyntax program;

    public override IEnumerable<Symbol> Members => members ??= BuildMembers();

    private ImmutableArray<Symbol>? members;

    public SourceModuleSymbol(ProgramSyntax program)
    {
        this.program = program;
    }

    private ImmutableArray<Symbol> BuildMembers()
    {
        var result = ImmutableArray.CreateBuilder<Symbol>();

        foreach (var syntax in program.Declarations)
        {
            if (syntax is FunctionDeclarationSyntax functionSyntax)
            {
                result.Add(new SourceFunctionSymbol(functionSyntax));
            }
            if (syntax is VariableDeclarationSyntax variable)
            {
                // TODO
                throw new NotImplementedException();
            }
        }

        return result.ToImmutable();
    }
}

public sealed class SourceFunctionSymbol : FunctionSymbol
{
    public override ImmutableArray<ParameterSymbol> Parameters => throw new NotImplementedException();

    public override TypeSymbol ReturnType => throw new NotImplementedException();

    public override string Name => functionSyntax.Name.Text;

    private readonly FunctionDeclarationSyntax functionSyntax;

    public SourceFunctionSymbol(FunctionDeclarationSyntax functionSyntax)
    {
        this.functionSyntax = functionSyntax;
    }
}
