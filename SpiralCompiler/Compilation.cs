using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiralCompiler.Binding;
using SpiralCompiler.Symbols;
using SpiralCompiler.Syntax;

namespace SpiralCompiler;

public sealed class Compilation
{
    internal List<ErrorMessage> BinderErrors { get; } = new();

    public ParseTree Syntax { get; }

    public ModuleSymbol RootModule => rootModule ??= new SourceModuleSymbol(Syntax.Root, null, this);

    private ModuleSymbol? rootModule;

    public BinderCache BinderCache { get; }

    public Compilation(ParseTree syntax)
    {
        Syntax = syntax;
        BinderCache = new(this);
    }

    public IEnumerable<ErrorMessage> GetErrors()
    {
        var errors = new List<ErrorMessage>();
        ((ISourceSymbol)RootModule).Bind(errors);
        return Syntax.Errors
            .Concat(BinderErrors)
            .Concat(errors).ToList();
    }

    public TypeSymbol TypeOf(ExpressionSyntax syntax)
    {
        var binder = BinderCache.GetBinder(syntax);
        var expr = binder.BindExpression(syntax, new());
        try
        {
            return expr.Type;
        }
        catch
        {
            return BuiltInTypeSymbol.Error;
        }
    }
}
