using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.BoundTree;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Symbols;

public sealed class SourceModuleSymbol : ModuleSymbol
{
    public override Compilation Compilation { get; }

    public override Symbol? ContainingSymbol { get; }

    public override IEnumerable<Symbol> Members => members ??= BuildMembers();

    public ProgramSyntax Syntax { get; }

    private ImmutableArray<Symbol>? members;

    public SourceModuleSymbol(ProgramSyntax program, Symbol? containingSymbol, Compilation compilation)
    {
        this.Syntax = program;
        ContainingSymbol = containingSymbol;
        Compilation = compilation;
    }

    private ImmutableArray<Symbol> BuildMembers()
    {
        var result = ImmutableArray.CreateBuilder<Symbol>();

        foreach (var syntax in Syntax.Declarations)
        {
            if (syntax is FunctionDeclarationSyntax functionSyntax)
            {
                result.Add(new SourceFunctionSymbol(functionSyntax, this));
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
    public override Symbol? ContainingSymbol { get; }

    public override string Name => Syntax.Name.Text;

    public override ImmutableArray<ParameterSymbol> Parameters => parameters ??= BuildParameters();
    private ImmutableArray<ParameterSymbol>? parameters;

    public override TypeSymbol ReturnType => throw new NotImplementedException();

    public BoundStatement Body => body ??= BindBody();

    private BoundStatement? body;

    public FunctionDeclarationSyntax Syntax { get; }

    public SourceFunctionSymbol(FunctionDeclarationSyntax functionSyntax, Symbol? containingSymbol)
    {
        this.Syntax = functionSyntax;
        ContainingSymbol = containingSymbol;
    }

    private ImmutableArray<ParameterSymbol> BuildParameters() => Syntax.Parameters.Values
        .Select(p => new SourceParameterSymbol(p))
        .Cast<ParameterSymbol>()
        .ToImmutableArray();

    private BoundStatement BindBody()
    {
        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindStatement(Syntax.Block);
    }
}

public sealed class SourceLocalVariableSymbol : LocalVariableSymbol
{
    public VariableDeclarationSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public override TypeSymbol Type => throw new NotImplementedException();

    public SourceLocalVariableSymbol(VariableDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }
}

public sealed class SourceParameterSymbol : ParameterSymbol
{
    public ParameterSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public override TypeSymbol Type => throw new NotImplementedException();

    public SourceParameterSymbol(ParameterSyntax syntax)
    {
        Syntax = syntax;
    }
}