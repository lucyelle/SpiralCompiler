using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using SpiralCompiler.Syntax;

namespace SpiralCompiler.Semantics;

public class TypeChechingBase : AstVisitorBase<Symbol.Type?>
{
    protected override Symbol.Type? VisitIdentifierTypeReference(TypeReference.Identifier node)
    {
        if (node.Symbol is null)
        {
            throw new InvalidOperationException();
        }
        return (Symbol.Type)node.Symbol;
    }
}

public sealed class TypeCheckingStage1 : TypeChechingBase
{
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

    protected override Symbol.Type? VisitFunctionDefStatement(Statement.FunctionDef node)
    {
        var paramTypes = new List<Symbol.Type>();
        foreach (var p in node.Params)
        {
            paramTypes.Add(VisitParameter(p) ?? throw new InvalidOperationException("parameter type is null"));
        }

        Symbol.Type? returnType = null;
        if (node.ReturnType is null)
        {
            returnType = BuiltInTypes.Void;
        }
        else
        {
            returnType = VisitTypeReference(node.ReturnType!) ?? throw new InvalidOperationException("visited type reference is null");
        }

        node.Symbol!.ReturnType = returnType;
        node.Symbol!.SymbolType = new Symbol.Type.Function(paramTypes, returnType);

        return null;
    }
}

public sealed class TypeCheckingStage2 : TypeChechingBase
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
        var leftType = VisitExpression(node.Left) ?? throw new InvalidOperationException();
        var rightType = VisitExpression(node.Right) ?? throw new InvalidOperationException();

        if (node.Op is BinOp.Or or BinOp.And)
        {
            if (leftType != BuiltInTypes.Boolean || rightType != BuiltInTypes.Boolean)
            {
                throw new InvalidOperationException("operator mismatch");
            }
            return BuiltInTypes.Boolean;
        }
        else if (node.Op is BinOp.Modulo)
        {
            if (leftType != BuiltInTypes.Int || rightType != BuiltInTypes.Int)
            {
                throw new InvalidOperationException("operator mismatch");
            }
            return BuiltInTypes.Int;
        }
        else if (node.Op is BinOp.Add)
        {
            if (leftType == BuiltInTypes.String && rightType == BuiltInTypes.String)
            {
                return BuiltInTypes.String;
            }
            else if (Symbol.Type.IsNumeric(leftType) && Symbol.Type.IsNumeric(rightType))
            {
                return Symbol.Type.CommonType(leftType, rightType);
            }
            else
            {
                throw new InvalidOperationException("operator '+' not defined on the given type");
            }
        }
        else if (node.Op is BinOp.AddAssign)
        {
            if (leftType == BuiltInTypes.String && rightType == BuiltInTypes.String)
            {
                return BuiltInTypes.String;
            }
            else if (leftType == BuiltInTypes.Int && rightType == BuiltInTypes.Int)
            {
                return BuiltInTypes.Int;
            }
            else if (leftType == BuiltInTypes.Double && Symbol.Type.IsNumeric(rightType))
            {
                return BuiltInTypes.Double;
            }
            else
            {
                throw new InvalidOperationException("operator '+=' not defined on the given type");
            }
        }
        else if (node.Op is BinOp.Substract or BinOp.Multiply or BinOp.Divide)
        {
            if (Symbol.Type.IsNumeric(leftType) && Symbol.Type.IsNumeric(rightType))
            {
                return Symbol.Type.CommonType(leftType, rightType);
            }
            else
            {
                throw new InvalidOperationException("operator mismatch");
            }
        }
        else if (node.Op is BinOp.Assign)
        {
            if (leftType == Symbol.Type.CommonType(leftType, rightType))
            {
                return leftType;
            }
            else
            {
                throw new InvalidOperationException("operator mismatch");
            }
        }
        else if (node.Op is BinOp.SubtractAssign or BinOp.MultiplyAssign or BinOp.DivideAssign)
        {
            if (leftType == BuiltInTypes.Int && rightType == BuiltInTypes.Int)
            {
                return BuiltInTypes.Int;
            }
            else if (leftType == BuiltInTypes.Double && Symbol.Type.IsNumeric(rightType))
            {
                return BuiltInTypes.Double;
            }
            else
            {
                throw new InvalidOperationException("operator mismatch");
            }
        }
        else if (node.Op is BinOp.Less or BinOp.LessEquals or BinOp.Greater or BinOp.GreaterEquals)
        {
            if (Symbol.Type.IsNumeric(leftType) && Symbol.Type.IsNumeric(rightType))
            {
                return BuiltInTypes.Boolean;
            }
            else
            {
                throw new InvalidOperationException("operator mismatch");
            }
        }
        else if (node.Op is BinOp.Equals or BinOp.NotEqual)
        {
            _ = Symbol.Type.CommonType(leftType, rightType);
            return BuiltInTypes.Boolean;
        }
        else
        {
            throw new InvalidOperationException("operator not found");
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
        var rightType = VisitExpression(node.Right) ?? throw new InvalidOperationException();

        if (node.Op is UnOpPre.Not)
        {
            if (rightType == BuiltInTypes.Boolean)
            {
                return BuiltInTypes.Boolean;
            }
            else
            {
                throw new InvalidOperationException("operator mismatch");
            }
        }
        else
        {
            if (Symbol.Type.IsNumeric(rightType))
            {
                return rightType;
            }
            else
            {
                throw new InvalidOperationException("operator mismatch");
            }
        }
    }

    protected override Symbol.Type? VisitFunctionCallExpression(Expression.FunctionCall node)
    {
        if (node.Symbols is null)
        {
            var functionType = (Symbol.Type.Function?)VisitExpression(node.Function)
                                ?? throw new InvalidOperationException("type checking called function didn't yield the type");

            if (node.Args.Count != functionType.ParamTypes.Count)
            {
                throw new InvalidOperationException("called function doesn't have the correct number of args");
            }

            foreach (var (arg, paramType) in node.Args.Zip(functionType.ParamTypes))
            {
                var argType = VisitExpression(arg) ?? throw new InvalidOperationException("argument type is null when function called");
                if (argType != paramType)
                {
                    throw new InvalidOperationException("argument type and parameter type not the same when function called");
                }
            }

            return functionType.ReturnType;
        }
        else
        {
            var argTypes = node.Args.Select(a => VisitExpression(a)).ToList();

            for (var i = 0; i < node.Symbols.Count;)
            {
                var symbol = node.Symbols[i];
                var funcType = (Symbol.Type.Function)((Symbol.ITyped)symbol).SymbolType!;

                if (funcType.ParamTypes.Count != argTypes.Count)
                {
                    node.Symbols.RemoveAt(i);
                    continue;
                }

                foreach (var (argType, paramType) in argTypes.Zip(funcType.ParamTypes))
                {
                    if (argType != paramType)
                    {
                        node.Symbols.RemoveAt(i);
                        goto continue_outer;
                    }
                }
                i++;
            continue_outer:;
            }

            if (node.Symbols.Count > 1)
            {
                throw new InvalidOperationException("too many fitting overloads");
            }
            else if (node.Symbols.Count == 0)
            {
                throw new InvalidOperationException("no fitting overload");
            }
            return ((Symbol.Type.Function)((Symbol.ITyped)node.Symbols[0]).SymbolType!).ReturnType;
        }
    }

    protected override Symbol.Type? VisitFunctionDefStatement(Statement.FunctionDef node)
    {
        currentReturnType = node.Symbol!.ReturnType;

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

    protected override Symbol.Type? VisitStringExpression(Expression.String node) => BuiltInTypes.String;
    protected override Symbol.Type? VisitIntegerExpression(Expression.Integer node) => BuiltInTypes.Int;
    protected override Symbol.Type? VisitBooleanExpression(Expression.Boolean node) => BuiltInTypes.Boolean;
    protected override Symbol.Type? VisitDoubleExpression(Expression.Double node) => BuiltInTypes.Double;
}
