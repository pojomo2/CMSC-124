using System.Reflection.Metadata;
using Ileto.Took;

namespace Ileto;

class Interpreter : Expr.IVisitor<object> {
    

    public object VisitGroupingExpr(Expr.Grouping expr)
    {
        return Evaluate(expr.Expression);
    }

    private object Evaluate(Expr expr)
    {
        return expr.Accept(this);
    }

    public object VisitBinaryExpr(Expr.Binary expr)
    {
        object left = Evaluate(expr.Left);
        object right = Evaluate(expr.Right);

        switch(expr.Operator.type)
        {
            case TokenType.GREATER:
                CheckNumberOperands(expr.Operator, left, right);
                return (double)left > (double)right;
            case TokenType.GREATER_EQUAL:
                return (double)left >= (double)right;
            case TokenType.LESS:
                return (double)left < (double)right;
            case TokenType.LESS_EQUAL:
                return (double)left <= (double)right;
            case TokenType.EQUAL_EQUAL:
                return IsEqual(left, right);
            case TokenType.BANG_EQUAL:
                return !IsEqual(left, right);
            case TokenType.MINUS:
                return (double)left - (double)right;
            case TokenType.PLUS:
                if (left is double && right is double)
                {
                    return (double)left + (double)right;
                }
                if (left is string && right is string)
                {
                    return (string)left + (string)right;
                }
                break;
            case TokenType.SLASH:
                return (double)left / (double)right;
            case TokenType.STAR:
                return (double)left * (double)right;
        }

        //unreachable
        return null;
    }

    public object VisitUnaryExpr(Expr.Unary expr)
    {
        object right = Evaluate(expr.Right);

        switch (expr.Operator.type)
        {
            case TokenType.BANG:
                return !IsTruthy(right);
             case TokenType.MINUS:
                CheckNumberOperand(expr.Operator, right);
                return -(double)right;
        }


        //unreachable
        return null;
    }

    private void CheckNumberOperand(Token op, object operand)
    {
        if (operand is double) return;
        throw new RuntimeError(op, "Operand must be a number");
    }

    private void CheckNumberOperands(Token op, object left, object right)
    {
        if (left is double && right is double) return;
        throw new RuntimeError(op, "Operand must be a number");
    }

    public object VisitLiteralExpr(Expr.Literal expr)
    {
        return expr.Value!;
    }

    private bool IsTruthy(object? value)
    {
        if (value is null) return false;
        if (value is bool booleanValue) return booleanValue;
        return true;
    }

    private bool IsEqual(object a, object b)
    {
        return object.Equals(a, b);
    }
}                                                                                                                                                                                                           