using System.Runtime.CompilerServices;
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
            throw new InvalidOperationException("type checker didn't fill out symbol type");
        }
        return symbol.SymbolType;
    }

    protected override Symbol.Type? VisitVarStatement(Statement.Var node)
    {
        var symbol = (Symbol.ITyped?)node.Symbol ?? throw new InvalidOperationException("var statement sybol is null");

        if (node.Type is null)
        {
            if (node.Value is null)
            {
                throw new InvalidOperationException("var type can't be null if value is not assigned");
            }
            var valueType = VisitExpression(node.Value);
            symbol.SymbolType = valueType;
        }
        else
        {
            var varType = VisitTypeReference(node.Type) ?? throw new InvalidOperationException();
            symbol.SymbolType = varType;

            if (node.Value is not null)
            {
                var valueType = VisitExpression(node.Value) ?? throw new InvalidOperationException();
                if (!Symbol.Type.IsAssignable(varType, valueType))
                {
                    throw new InvalidOperationException("var type and value type does not match");
                }
            }
        }

        return symbol.SymbolType;
    }

    protected override Symbol.Type? VisitBinaryExpression(Expression.Binary node)
    {
        var leftType = VisitExpression(node.Left);
        var rightType = VisitExpression(node.Right);

        if (node.Op is BinOp.Or or BinOp.And)
        {
            if (leftType != BuiltInTypes.Boolean || rightType != BuiltInTypes.Boolean)
            {
                throw new InvalidOperationException("operator mismatch");
            }
            return BuiltInTypes.Boolean;
        }
        else if (node.Op is BinOp.Add)
        {
            // TODO: string or numbers
        }
        else if (node.Op is BinOp.AddAssign)
        {
            // TODO: string or numbers
        }
        else if (node.Op is BinOp.Substract or BinOp.Multiply or BinOp.Divide)
        {
            // TODO: numbers
        }
        else if (node.Op is BinOp.Assign)
        {
            // TODO: assignable
        }
        else if (node.Op is BinOp.SubtractAssign or BinOp.MultiplyAssign or BinOp.DivideAssign)
        {

        }
        else if (node.Op is BinOp.Less or BinOp.LessEquals or BinOp.Greater or BinOp.GreaterEquals)
        {

        }
        else if (node.Op is BinOp.Equals or BinOp.NotEqual)
        {

        }
    }

    protected override Symbol.Type? VisitUnaryPostExpression(Expression.UnaryPost node)
    {
        var leftType = VisitExpression(node.Left) ?? throw new InvalidOperationException();
        if (!Symbol.Type.IsNumeric(leftType))
        {
            throw new InvalidOperationException("operation mismatch with int/double expression");
        }
        return leftType;
    }

    protected override Symbol.Type? VisitUnaryPreExpression(Expression.UnaryPre node)
    {
        var rightType = VisitExpression(node.Right);

        if (node.Op is UnOpPre.Not)
        {
            // TODO: bool
        }
        else
        {
            // TODO: numerics
        }

        return rightType;
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
