namespace SpiralCompiler.Syntax;
public abstract class AstVisitorBase<T>
{
    public T? VisitStatement(Statement stmtNode) => stmtNode switch
    {
        Statement.Class node => VisitClassStatement(node),
        Statement.For node => VisitForStatement(node),
        Statement.Expr node => VisitExprStatement(node),
        Statement.Return node => VisitReturnStatement(node),
        Statement.Block node => VisitBlockStatement(node),
        Statement.FunctionDef node => VisitFunctionDefStatement(node),
        Statement.If node => VisitIfStatement(node),
        Statement.Var node => VisitVarStatement(node),
        Statement.While node => VisitWhileStatement(node),
        _ => throw new ArgumentOutOfRangeException(nameof(stmtNode)),
    };
    protected virtual T? VisitClassStatement(Statement.Class node)
    {
        foreach (var field in node.Fields)
        {
            VisitField(field);
        }
        foreach (var func in node.Functions)
        {
            VisitFunctionDefStatement(func);
        }
        return default;
    }

    protected virtual T? VisitWhileStatement(Statement.While node)
    {
        VisitExpression(node.Condition);
        VisitStatement(node.Body);
        return default;
    }
    protected virtual T? VisitVarStatement(Statement.Var node)
    {
        if (node.Type is not null)
        {
            VisitTypeReference(node.Type);
        }
        if (node.Value is not null)
        {
            VisitExpression(node.Value);
        }

        return default;
    }
    protected virtual T? VisitField(Statement.Field node)
    {
        VisitTypeReference(node.Type!);

        if (node.Value is not null)
        {
            VisitExpression(node.Value);
        }

        return default;
    }
    protected virtual T? VisitIfStatement(Statement.If node)
    {
        VisitExpression(node.Condition);
        VisitStatement(node.Then);
        if (node.Else is not null)
        {
            VisitStatement(node.Else);
        }
        return default;
    }

    protected virtual T? VisitFunctionDefStatement(Statement.FunctionDef node)
    {
        foreach (var p in node.Params)
        {
            VisitParameter(p);
        }
        if (node.ReturnType is not null)
        {
            VisitTypeReference(node.ReturnType);
        }
        VisitStatement(node.Body);
        return default;
    }

    protected virtual T? VisitParameter(Parameter param)
    {
        VisitTypeReference(param.Type);
        return default;
    }

    protected T? VisitTypeReference(TypeReference type) => type switch
    {
        TypeReference.Identifier node => VisitIdentifierTypeReference(node),
        _ => throw new ArgumentOutOfRangeException(nameof(type)),
    };

    protected virtual T? VisitIdentifierTypeReference(TypeReference.Identifier node)
    {
        return default;
    }

    protected virtual T? VisitBlockStatement(Statement.Block node)
    {
        foreach (var s in node.Statements)
        {
            VisitStatement(s);
        }
        return default;
    }

    protected virtual T? VisitReturnStatement(Statement.Return node)
    {
        if (node.Expression is not null)
        {
            VisitExpression(node.Expression);
        }
        return default;
    }
    protected virtual T? VisitForStatement(Statement.For node)
    {
        VisitExpression(node.Range);
        VisitStatement(node.Body);
        return default;
    }
    protected virtual T? VisitExprStatement(Statement.Expr node)
    {
        VisitExpression(node.Expression);
        return default;
    }

    public T? VisitExpression(Expression exprNode) => exprNode switch
    {
        Expression.Identifier node => VisitIdentifierExpression(node),
        Expression.Boolean node => VisitBooleanExpression(node),
        Expression.String node => VisitStringExpression(node),
        Expression.Integer node => VisitIntegerExpression(node),
        Expression.Double node => VisitDoubleExpression(node),
        Expression.Binary node => VisitBinaryExpression(node),
        Expression.UnaryPre node => VisitUnaryPreExpression(node),
        Expression.UnaryPost node => VisitUnaryPostExpression(node),
        Expression.FunctionCall node => VisitFunctionCallExpression(node),
        Expression.MemberAccess node => VisitMemberAccessExpression(node),
        Expression.New node => VisitNewEpression(node),
        _ => throw new ArgumentOutOfRangeException(nameof(exprNode)),
    };
    protected virtual T? VisitNewEpression(Expression.New node)
    {
        return default;
    }

    protected virtual T? VisitMemberAccessExpression(Expression.MemberAccess node)
    {
        VisitExpression(node.Left);
        return default;
    }

    protected virtual T? VisitFunctionCallExpression(Expression.FunctionCall node)
    {
        VisitExpression(node.Function);
        foreach (var p in node.Args)
        {
            VisitExpression(p);
        }
        return default;
    }

    protected virtual T? VisitUnaryPostExpression(Expression.UnaryPost node)
    {
        VisitExpression(node.Left);
        return default;
    }

    protected virtual T? VisitUnaryPreExpression(Expression.UnaryPre node)
    {
        VisitExpression(node.Right);
        return default;
    }

    protected virtual T? VisitBinaryExpression(Expression.Binary node)
    {
        VisitExpression(node.Left);
        VisitExpression(node.Right);
        return default;
    }

    protected virtual T? VisitDoubleExpression(Expression.Double node)
    {
        return default;
    }

    protected virtual T? VisitIntegerExpression(Expression.Integer node)
    {
        return default;
    }

    protected virtual T? VisitStringExpression(Expression.String node)
    {
        return default;
    }

    protected virtual T? VisitBooleanExpression(Expression.Boolean node)
    {
        return default;
    }

    protected virtual T? VisitIdentifierExpression(Expression.Identifier node)
    {
        return default;
    }
}
