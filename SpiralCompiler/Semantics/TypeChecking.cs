using SpiralCompiler.Syntax;

namespace SpiralCompiler.Semantics;
public class TypeChecking : AstVisitorBase<Symbol.Type?>
{
    private Symbol.Type? currentReturnType;

    protected override Symbol.Type VisitIdentifierExpression(Expression.Identifier node)
    {
        if (node.Symbol is null)
        {
            throw new ArgumentException("AST node symbol must be filled out", nameof(node));
        }
        var symbol = (Symbol.ITyped)node.Symbol;
        if (symbol.SymbolType is null)
        {
            throw new InvalidOperationException("type checker didnt fill out symbol type");
        }
        return symbol.SymbolType;
    }

    protected override Symbol.Type? VisitFunctionCallExpression(Expression.FunctionCall node)
    {
        var functionType = (Symbol.Type.Function?)VisitExpression(node.Function)
            ?? throw new InvalidOperationException("type checking called function didn't yield the type");

        if (node.Params.Count != functionType.ParamTypes.Count)
        {
            throw new InvalidOperationException("called function doesn't have the correct number of args");
        }

        foreach (var (arg, paramType) in node.Params.Zip(functionType.ParamTypes))
        {
            var argType = VisitExpression(arg) ?? throw new InvalidOperationException("argument type is null when function called");
            if (argType != paramType)
            {
                throw new InvalidOperationException("argument type and parameter type not the same when function called");
            }
        }

        return functionType.ReturnType;
    }

    protected override Symbol.Type? VisitFunctionDefStatement(Statement.FunctionDef node)
    {
        var paramTypes = new List<Symbol.Type>();
        foreach (var p in node.Params)
        {
            paramTypes.Add(VisitParameter(p) ?? throw new InvalidOperationException("parameter type is null"));
        }

        if (node.ReturnType is null)
        {
            currentReturnType = BuiltInTypes.Void;
        }
        else
        {
            currentReturnType = VisitTypeReference(node.ReturnType!) ?? throw new InvalidOperationException("visited type reference is null");
        }
        node.Symbol!.ReturnType = currentReturnType;

        node.Symbol.SymbolType = new Symbol.Type.Function(paramTypes, currentReturnType);

        VisitStatement(node.Body);

        return null;
    }

    protected override Symbol.Type? VisitReturnStatement(Statement.Return node)
    {
        if (node.Expression is null)
        {
            if (currentReturnType != BuiltInTypes.Void)
            {
                throw new InvalidOperationException("return type mismatch");
            }
        }
        else
        {
            var returnType = VisitExpression(node.Expression) ?? throw new InvalidOperationException();
            if (returnType != currentReturnType)
            {
                throw new InvalidOperationException("return type mismatch");
            }
        }
        return null;
    }

    protected override Symbol.Type? VisitParameter(Parameter param)
    {
        var type = VisitTypeReference(param.Type) ?? throw new InvalidOperationException("parameter type is null");
        if (param.Symbol is null)
        {
            throw new InvalidOperationException("parameter symbol is null");
        }
        param.Symbol.SymbolType = type;

        return type;
    }

    protected override Symbol.Type? VisitIdentifierTypeReference(TypeReference.Identifier node)
    {
        if (node.Symbol is null)
        {
            throw new InvalidOperationException();
        }
        return (Symbol.Type)node.Symbol;
    }

    protected override Symbol.Type? VisitStringExpression(Expression.String node) => BuiltInTypes.String;

    protected override Symbol.Type? VisitIntegerExpression(Expression.Integer node) => BuiltInTypes.Int;
}
