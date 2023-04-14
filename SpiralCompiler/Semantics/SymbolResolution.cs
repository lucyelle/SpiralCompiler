using SpiralCompiler.Syntax;

namespace SpiralCompiler.Semantics;
public sealed class SymbolResolutionStage1 : AstVisitorBase<Unit>
{
    private Scope currentScope;
    public Scope RootScope { get; }

    public static Statement SmybolResStage1(Statement ast, out SymbolResolutionStage1 stage1)
    {
        // TODO
        stage1 = new SymbolResolutionStage1();
        stage1.VisitStatement(ast);
        return ast;
    }

    public SymbolResolutionStage1()
    {
        RootScope = new Scope(null);
        currentScope = RootScope;
        RootScope.AddSymbol(BuiltInTypes.Int);
        RootScope.AddSymbol(BuiltInTypes.Boolean);
        RootScope.AddSymbol(BuiltInTypes.Double);
        RootScope.AddSymbol(BuiltInTypes.String);
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

public sealed class SymbolResolutionStage2 : AstVisitorBase<Unit>
{
    private Scope currentScope;

    public SymbolResolutionStage2(Scope root)
    {
        currentScope = root;
    }

    public static Statement SymbolResStage2(Statement ast)
    {
        var newAst = SymbolResolutionStage1.SmybolResStage1(ast, out var stage1);
        var stage2 = new SymbolResolutionStage2(stage1.RootScope);
        stage2.VisitStatement(newAst);
        return newAst;
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
