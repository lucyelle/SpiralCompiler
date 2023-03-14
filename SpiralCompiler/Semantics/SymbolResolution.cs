using SpiralCompiler.Syntax;

namespace SpiralCompiler.Semantics;
public class SymbolResolutionStage1 : AstVisitorBase<Unit>
{
    private Scope currentScope;
    public Scope RootScope { get; }

    public SymbolResolutionStage1()
    {
        RootScope = new Scope(null);
        currentScope = RootScope;
        RootScope.AddSymbol(new Symbol.Type.Primitive("string", typeof(string)));
        RootScope.AddSymbol(new Symbol.Type.Primitive("int", typeof(int)));
    }

    // TODO: typecheck: correct variable assignment, correct funtion call (params), correct return type, correct operator usage

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

    private void AddSymbol(Symbol symbol)
    {
        currentScope.AddSymbol(symbol);
    }
}

public class SymbolResolutionStage2 : AstVisitorBase<Unit>
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
        var symbol = currentScope.SearchSymbol(node.Name);
        node.Symbol = symbol;
        return default;
    }

    protected override Unit VisitIdentifierTypeReference(TypeReference.Identifier node)
    {
        var symbol = currentScope.SearchSymbol(node.Name);
        node.Symbol = symbol;
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
