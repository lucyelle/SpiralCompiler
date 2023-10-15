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
            else if (syntax is VariableDeclarationSyntax variableSyntax)
            {
                // TODO
                throw new NotImplementedException();
            }
            else if (syntax is ClassDeclarationSyntax classSyntax)
            {
                result.Add(new SourceClassSymbol(classSyntax, this));
            }
            else if (syntax is InterfaceDeclarationSyntax interfaceSyntax)
            {
                result.Add(new SourceInterfaceSymbol(interfaceSyntax, this));
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

    public override TypeSymbol ReturnType => returnType ??= BuildReturnType();
    private TypeSymbol? returnType;

    public BoundStatement Body => body ??= BindBody();
    private BoundStatement? body;

    public FunctionDeclarationSyntax Syntax { get; }

    public SourceFunctionSymbol(FunctionDeclarationSyntax functionSyntax, Symbol? containingSymbol)
    {
        this.Syntax = functionSyntax;
        ContainingSymbol = containingSymbol;
    }

    private ImmutableArray<ParameterSymbol> BuildParameters() => Syntax.Parameters.Values
        .Select(p => new SourceParameterSymbol(this, p))
        .Cast<ParameterSymbol>()
        .ToImmutableArray();

    private TypeSymbol BuildReturnType()
    {
        if (Syntax.ReturnType is null) return BuiltInTypeSymbol.Void;

        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindType(Syntax.ReturnType.Type);
    }

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

    public override TypeSymbol Type => type ?? throw new InvalidOperationException();
    private TypeSymbol? type;

    public SourceLocalVariableSymbol(VariableDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public void SetType(TypeSymbol newType)
    {
        if (type is not null) throw new InvalidOperationException();
        type = newType;
    }
}

public sealed class SourceParameterSymbol : ParameterSymbol
{
    public override Symbol? ContainingSymbol { get; }

    public ParameterSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public override TypeSymbol Type => type ??= BuildType();

    private TypeSymbol? type;

    public SourceParameterSymbol(Symbol? containingSymbol, ParameterSyntax syntax)
    {
        Syntax = syntax;
        ContainingSymbol = containingSymbol;
    }

    private TypeSymbol BuildType()
    {
        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindType(Syntax.Type);
    }
}

public sealed class SourceInterfaceSymbol : InterfaceSymbol
{
    public override Symbol? ContainingSymbol { get; }

    public InterfaceDeclarationSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public SourceInterfaceSymbol(InterfaceDeclarationSyntax syntax, Symbol? containingSymbol)
    {
        ContainingSymbol = containingSymbol;
        Syntax = syntax;
    }
}

public sealed class SourceClassSymbol : ClassSymbol
{
    public override Symbol? ContainingSymbol { get; }

    public ClassDeclarationSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public override IEnumerable<Symbol> Members => members ??= BuildMembers();
    private ImmutableArray<Symbol>? members;

    public override OverloadSymbol Constructors => constructors ??= BuildConstructors();
    private OverloadSymbol? constructors;

    public override IEnumerable<FieldSymbol> Fields => fields ??= BuildFields();
    private ImmutableArray<FieldSymbol>? fields;

    public SourceClassSymbol(ClassDeclarationSyntax syntax, Symbol? containingSymbol)
    {
        ContainingSymbol = containingSymbol;
        Syntax = syntax;
    }

    private ImmutableArray<Symbol> BuildMembers()
    {
        var result = ImmutableArray.CreateBuilder<Symbol>();

        foreach (var syntax in Syntax.Members)
        {
            if (syntax is FieldDeclarationSyntax field)
            {
                result.Add(new SourceFieldSymbol(field, this));
            }
            else
            {
                // TODO
                throw new NotImplementedException();
            }
        }

        return result.ToImmutable();
    }

    private OverloadSymbol BuildConstructors()
    {
        var ctorFunctions = Members
            .OfType<FunctionSymbol>()
            .Where(f => f.IsConstructor)
            .ToImmutableArray()
            .ToBuilder();

        if (ctorFunctions.Count == 0)
        {
            ctorFunctions.Add(new SynthetizedDefaultConstructorSymbol(this));
        }

        return new(ctorFunctions.ToImmutable());
    }

    private ImmutableArray<FieldSymbol> BuildFields() => Members.OfType<FieldSymbol>().ToImmutableArray();
}

public sealed class SourceFieldSymbol : FieldSymbol
{
    public override Symbol? ContainingSymbol { get; }
    public override string Name => base.Name;

    public FieldDeclarationSyntax Syntax { get; }

    public override TypeSymbol Type => type ??= BuildType();
    private TypeSymbol? type;

    public SourceFieldSymbol(FieldDeclarationSyntax syntax, Symbol? containingSymbol)
    {
        Syntax = syntax;
        ContainingSymbol = containingSymbol;
    }

    private TypeSymbol BuildType()
    {
        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindType(Syntax.Type);
    }
}
