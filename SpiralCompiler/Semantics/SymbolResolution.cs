using SpiralCompiler.Syntax;

namespace SpiralCompiler.Semantics;
public sealed class SymbolResolutionStage1 : AstVisitorBase<Unit>
{
    private Scope currentScope;
    public Scope RootScope { get; }

    public SymbolResolutionStage1()
    {
        RootScope = new Scope(null);
        currentScope = RootScope;
        RootScope.AddSymbol(BuiltInTypes.Int);
        RootScope.AddSymbol(BuiltInTypes.Boolean);
        RootScope.AddSymbol(BuiltInTypes.Double);
        RootScope.AddSymbol(BuiltInTypes.String);

        foreach (var func in BuiltInFunctions.Delegates.Keys)
        {
            RootScope.AddSymbol(func);
        }
    }

    protected override Unit VisitFunctionDefStatement(Statement.FunctionDef node)
    {
        node.Scope = PushScope();
        base.VisitFunctionDefStatement(node);
        PopScope();

        var parameters = node.Params.Select(p => p.Symbol).Cast<Symbol.Variable>().ToList();
        var symbol = new Symbol.Function(node.Name, parameters);
        node.Symbol = symbol;
        AddSymbol(symbol);

        return default;
    }

    protected override Unit VisitClassStatement(Statement.Class node)
    {
        node.Scope = PushScope();
        base.VisitClassStatement(node);
        PopScope();

        var fields = node.Fields.Select(f => f.Symbol).Cast<Symbol.Variable>().ToList();
        var functions = node.Functions.Select(f => f.Symbol).Cast<Symbol.Function>().ToList();
        var symbol = new Symbol.Type.Class(node.Name, fields, functions, new());
        node.Symbol = symbol;
        AddSymbol(symbol);

        return default;
    }

    protected override Unit VisitField(Statement.Field node)
    {
        base.VisitField(node);

        var symbol = new Symbol.Variable(node.Name);
        node.Symbol = symbol;
        AddSymbol(symbol);

        return default;
    }

    protected override Unit VisitBlockStatement(Statement.Block node)
    {
        node.Scope = PushScope();
        base.VisitBlockStatement(node);
        PopScope();

        return default;
    }

    protected override Unit VisitVarStatement(Statement.Var node)
    {
        base.VisitVarStatement(node);

        if (currentScope == RootScope)
        {
            var symbol = new Symbol.Variable(node.Name);
            node.Symbol = symbol;
            AddSymbol(symbol);
        }

        return default;
    }

    protected override Unit VisitParameter(Parameter param)
    {
        base.VisitParameter(param);

        var symbol = new Symbol.Variable(param.Name);
        param.Symbol = symbol;
        AddSymbol(symbol);

        return default;
    }

    private Scope PushScope()
    {
        currentScope = new Scope(currentScope);
        return currentScope;
    }

    private void PopScope()
    {
        if (currentScope.Parent is null)
        {
            throw new InvalidOperationException();
        }
        currentScope = currentScope.Parent;
    }

    private void AddSymbol(Symbol symbol) => currentScope.AddSymbol(symbol);
}

public sealed class SymbolResolutionStage2 : AstVisitorBase<Unit>
{
    private Scope currentScope;

    public SymbolResolutionStage2(Scope root)
    {
        currentScope = root;
    }

    protected override Unit VisitFunctionDefStatement(Statement.FunctionDef node)
    {
        PushScope(node.Scope);
        base.VisitFunctionDefStatement(node);
        PopScope();

        return default;
    }

    protected override Unit VisitClassStatement(Statement.Class node)
    {
        PushScope(node.Scope);
        base.VisitClassStatement(node);
        PopScope();

        // TODO: store multiple bases in AST
        if (node.Base is null)
        {
            return default;
        }
        var baseSymbol = currentScope.SearchSymbol(node.Base).Single();
        node.Symbol!.Bases.Add(baseSymbol);

        return default;
    }

    protected override Unit VisitMemberAccessExpression(Expression.MemberAccess node)
    {
        base.VisitMemberAccessExpression(node);

        // ez így nem lesz jó
        var memberSymbol = currentScope.SearchSymbol(node.MemberName).Single();
        node.Symbol = memberSymbol;

        // TODO: which object -> which class

        return default;
    }

    protected override Unit VisitFunctionCallExpression(Expression.FunctionCall node)
    {
        if (node.Function is Expression.Identifier id)
        {
            node.Symbols = currentScope.SearchSymbol(id.Name);
            foreach (var arg in node.Args)
            {
                VisitExpression(arg);
            }
        }
        else
        {
            base.VisitFunctionCallExpression(node);
        }

        return default;
    }

    protected override Unit VisitBlockStatement(Statement.Block node)
    {
        PushScope(node.Scope);
        base.VisitBlockStatement(node);
        PopScope();

        return default;
    }

    protected override Unit VisitVarStatement(Statement.Var node)
    {
        base.VisitVarStatement(node);
        if (currentScope.Parent is not null)
        {
            var symbol = new Symbol.Variable(node.Name);
            node.Symbol = symbol;
            AddSymbol(symbol);
        }

        return default;
    }

    protected override Unit VisitIdentifierExpression(Expression.Identifier node)
    {
        var symbols = currentScope.SearchSymbol(node.Name);
        node.Symbol = symbols.Single();
        return default;
    }

    protected override Unit VisitIdentifierTypeReference(TypeReference.Identifier node)
    {
        var symbols = currentScope.SearchSymbol(node.Name);
        node.Symbol = symbols.Single();
        return default;
    }

    private void PushScope(Scope? scope)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }
        currentScope = scope;
    }

    private void PopScope()
    {
        if (currentScope.Parent is null)
        {
            throw new InvalidOperationException();
        }
        currentScope = currentScope.Parent;
    }

    private void AddSymbol(Symbol symbol)
    {
        currentScope.AddSymbol(symbol);
    }
}
