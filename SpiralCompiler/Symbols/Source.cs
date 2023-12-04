using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Binding;
using SpiralCompiler.BoundTree;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Symbols;

public interface ISourceSymbol
{
    public void Bind(List<ErrorMessage> errors);
}

public sealed class SourceModuleSymbol : ModuleSymbol, ISourceSymbol
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
                result.Add(new SourceGlobalVariableSymbol(variableSyntax, this));
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

    public void Bind(List<ErrorMessage> errors)
    {
        foreach (var m in Members.OfType<ISourceSymbol>())
        {
            m.Bind(errors);
        }
    }
}

public sealed class SourceFunctionSymbol : FunctionSymbol, ISourceSymbol
{
    public override Symbol? ContainingSymbol { get; }

    public override string Name => Syntax.Name.Text;
    public override bool IsInstance => ContainingSymbol is ClassSymbol;

    public override FunctionSymbol BaseDeclaration => baseDeclaration ??= BuildBaseDeclaration();
    private FunctionSymbol? baseDeclaration;

    public override ImmutableArray<ParameterSymbol> Parameters => parameters ??= BuildParameters();
    private ImmutableArray<ParameterSymbol>? parameters;

    public override TypeSymbol ReturnType => returnType ??= BuildReturnType(Compilation.BinderErrors);
    private TypeSymbol? returnType;

    public BoundStatement Body => body ??= BindBody(Compilation.BinderErrors);
    private BoundStatement? body;

    public FunctionDeclarationSyntax Syntax { get; }

    public SourceFunctionSymbol(FunctionDeclarationSyntax functionSyntax, Symbol? containingSymbol)
    {
        this.Syntax = functionSyntax;
        ContainingSymbol = containingSymbol;
    }

    public void Bind(List<ErrorMessage> errors)
    {
        BuildReturnType(errors);
        BindBody(errors);
    }

    private FunctionSymbol BuildBaseDeclaration()
    {
        if (ContainingSymbol is not ClassSymbol classSymbol) return this;

        foreach (var iface in classSymbol.BaseTypes.OfType<InterfaceSymbol>())
        {
            foreach (var member in iface.Members.OfType<FunctionSymbol>())
            {
                if (SignatureEquals(member)) return member;
            }
        }

        return this;
    }

    private ImmutableArray<ParameterSymbol> BuildParameters() => Syntax.Parameters.Values
        .Select(p => new SourceParameterSymbol(this, p))
        .Cast<ParameterSymbol>()
        .ToImmutableArray();

    private TypeSymbol BuildReturnType(List<ErrorMessage> errors)
    {
        if (Syntax.ReturnType is null) return BuiltInTypeSymbol.Void;

        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindType(Syntax.ReturnType.Type, errors);
    }

    private BoundStatement BindBody(List<ErrorMessage> errors)
    {
        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindStatement(Syntax.Block, errors);
    }
}

public sealed class SourceFunctionSignatureSymbol : FunctionSymbol, ISourceSymbol
{
    public override Symbol? ContainingSymbol { get; }

    public override string Name => Syntax.Name.Text;
    public override bool IsInstance => true;

    public override ImmutableArray<ParameterSymbol> Parameters => parameters ??= BuildParameters();
    private ImmutableArray<ParameterSymbol>? parameters;

    public override TypeSymbol ReturnType => returnType ??= BuildReturnType(Compilation.BinderErrors);
    private TypeSymbol? returnType;

    public MethodSignatureSyntax Syntax { get; }

    public SourceFunctionSignatureSymbol(MethodSignatureSyntax functionSyntax, Symbol? containingSymbol)
    {
        Syntax = functionSyntax;
        ContainingSymbol = containingSymbol;
    }

    public void Bind(List<ErrorMessage> errors)
    {
        BuildReturnType(errors);
    }

    private ImmutableArray<ParameterSymbol> BuildParameters() => Syntax.Parameters.Values
        .Select(p => new SourceParameterSymbol(this, p))
        .Cast<ParameterSymbol>()
        .ToImmutableArray();

    private TypeSymbol BuildReturnType(List<ErrorMessage> errors)
    {
        if (Syntax.ReturnType is null) return BuiltInTypeSymbol.Void;

        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindType(Syntax.ReturnType.Type, errors);
    }
}

public sealed class SourceGlobalVariableSymbol : GlobalVariableSymbol, ISourceSymbol
{
    public override Symbol? ContainingSymbol { get; }
    public VariableDeclarationSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public override TypeSymbol Type
    {
        get
        {
            if (NeedsBinding) Bind(Compilation.BinderErrors);
            return type!;
        }
    }
    public override BoundExpression? InitialValue
    {
        get
        {
            if (NeedsBinding) Bind(Compilation.BinderErrors);
            return initialValue;
        }
    }

    private bool NeedsBinding => type is null;
    private TypeSymbol? type;
    private BoundExpression? initialValue;

    public SourceGlobalVariableSymbol(VariableDeclarationSyntax syntax, Symbol? containingSymbol)
    {
        Syntax = syntax;
        ContainingSymbol = containingSymbol;
    }

    public void Bind(List<ErrorMessage> errors)
    {
        if (Syntax.Type is null && Syntax.Value is null)
        {
            // TODO: error
            throw new NotImplementedException("a global must have at least a type or a value");
        }

        var binder = Compilation.BinderCache.GetBinder(Syntax);

        if (Syntax.Type is not null)
        {
            type = binder.BindType(Syntax.Type.Type, errors);
        }

        if (Syntax.Value is not null)
        {
            initialValue = binder.BindExpression(Syntax.Value.Value, errors);
            if (type is not null)
            {
                TypeSystem.Assignable(type, initialValue.Type);
            }
            else
            {
                type = initialValue.Type;
            }
        }
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

public sealed class SourceParameterSymbol : ParameterSymbol, ISourceSymbol
{
    public override Symbol? ContainingSymbol { get; }

    public ParameterSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public override TypeSymbol Type => type ??= BuildType(Compilation.BinderErrors);

    private TypeSymbol? type;

    public SourceParameterSymbol(Symbol? containingSymbol, ParameterSyntax syntax)
    {
        Syntax = syntax;
        ContainingSymbol = containingSymbol;
    }

    public void Bind(List<ErrorMessage> errors)
    {
        BuildType(errors);
    }

    private TypeSymbol BuildType(List<ErrorMessage> errors)
    {
        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindType(Syntax.Type, errors);
    }
}

public sealed class SourceInterfaceSymbol : InterfaceSymbol, ISourceSymbol
{
    public override Symbol? ContainingSymbol { get; }

    public InterfaceDeclarationSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public override IEnumerable<TypeSymbol> ImmediateBaseTypes => immediateBaseTypes ??= BuildImmediateBaseTypes(Compilation.BinderErrors);
    private ImmutableArray<TypeSymbol>? immediateBaseTypes;

    public override IEnumerable<Symbol> Members => members ??= BuildMembers();
    private ImmutableArray<Symbol>? members;

    public SourceInterfaceSymbol(InterfaceDeclarationSyntax syntax, Symbol? containingSymbol)
    {
        ContainingSymbol = containingSymbol;
        Syntax = syntax;
    }

    public void Bind(List<ErrorMessage> errors)
    {
        BuildImmediateBaseTypes(errors);
        foreach (var m in Members.OfType<ISourceSymbol>())
        {
            m.Bind(errors);
        }
    }

    private ImmutableArray<TypeSymbol> BuildImmediateBaseTypes(List<ErrorMessage> errors)
    {
        if (Syntax.Bases is null) return ImmutableArray<TypeSymbol>.Empty;

        var binder = Compilation.BinderCache.GetBinder(Syntax.Bases);
        var result = ImmutableArray.CreateBuilder<TypeSymbol>();
        foreach (var baseSyntax in Syntax.Bases.Bases.Values)
        {
            result.Add(binder.BindType(baseSyntax, errors));
        }
        return result.ToImmutable();
    }

    private ImmutableArray<Symbol> BuildMembers()
    {
        var result = ImmutableArray.CreateBuilder<Symbol>();

        foreach (var syntax in Syntax.Members)
        {
            if (syntax is MethodSignatureSyntax signature)
            {
                result.Add(new SourceFunctionSignatureSymbol(signature, this));
            }
            else
            {
                // TODO
                throw new NotImplementedException();
            }
        }

        return result.ToImmutable();
    }
}

public sealed class SourceClassSymbol : ClassSymbol, ISourceSymbol
{
    public override Symbol? ContainingSymbol { get; }

    public ClassDeclarationSyntax Syntax { get; }

    public override string Name => Syntax.Name.Text;

    public override IEnumerable<TypeSymbol> ImmediateBaseTypes => immediateBaseTypes ??= BuildImmediateBaseTypes(Compilation.BinderErrors);
    private ImmutableArray<TypeSymbol>? immediateBaseTypes;

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

    public void Bind(List<ErrorMessage> errors)
    {
        BuildImmediateBaseTypes(errors);
        foreach (var m in Members.OfType<ISourceSymbol>())
        {
            m.Bind(errors);
        }
    }

    private ImmutableArray<TypeSymbol> BuildImmediateBaseTypes(List<ErrorMessage> errors)
    {
        if (Syntax.Bases is null) return ImmutableArray<TypeSymbol>.Empty;

        var binder = Compilation.BinderCache.GetBinder(Syntax.Bases);
        var result = ImmutableArray.CreateBuilder<TypeSymbol>();
        foreach (var baseSyntax in Syntax.Bases.Bases.Values)
        {
            result.Add(binder.BindType(baseSyntax, errors));
        }
        return result.ToImmutable();
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
            else if (syntax is CtorDeclarationSyntax ctor)
            {
                result.Add(new SourceConstructorSymbol(ctor, this));
            }
            else if (syntax is FunctionDeclarationSyntax func)
            {
                result.Add(new SourceFunctionSymbol(func, this));
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

public sealed class SourceFieldSymbol : FieldSymbol, ISourceSymbol
{
    public override Symbol? ContainingSymbol { get; }
    public override string Name => Syntax.Name.Text;

    public FieldDeclarationSyntax Syntax { get; }

    public override TypeSymbol Type => type ??= BuildType(Compilation.BinderErrors);
    private TypeSymbol? type;

    public SourceFieldSymbol(FieldDeclarationSyntax syntax, Symbol? containingSymbol)
    {
        Syntax = syntax;
        ContainingSymbol = containingSymbol;
    }

    public void Bind(List<ErrorMessage> errors)
    {
        BuildType(errors);
    }

    private TypeSymbol BuildType(List<ErrorMessage> errors)
    {
        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindType(Syntax.Type, errors);
    }
}

public sealed class SourceConstructorSymbol : FunctionSymbol, ISourceSymbol
{
    public override TypeSymbol ContainingSymbol { get; }

    public CtorDeclarationSyntax Syntax { get; }

    public override ImmutableArray<ParameterSymbol> Parameters => parameters ??= BuildParameters();
    private ImmutableArray<ParameterSymbol>? parameters;

    public override TypeSymbol ReturnType => ContainingSymbol;

    public override bool IsConstructor => true;

    public override string Name => ".ctor";

    public BoundStatement Body => body ??= BindBody(Compilation.BinderErrors);
    private BoundStatement? body;

    public SourceConstructorSymbol(CtorDeclarationSyntax syntax, TypeSymbol containingSymbol)
    {
        Syntax = syntax;
        ContainingSymbol = containingSymbol;
    }

    public void Bind(List<ErrorMessage> errors)
    {
        BindBody(errors);
    }

    private ImmutableArray<ParameterSymbol> BuildParameters() => Syntax.Parameters.Values
        .Select(p => new SourceParameterSymbol(this, p))
        .Cast<ParameterSymbol>()
        .ToImmutableArray();

    private BoundStatement BindBody(List<ErrorMessage> errors)
    {
        var binder = Compilation.BinderCache.GetBinder(Syntax);
        return binder.BindStatement(Syntax.Block, errors);
    }
}
