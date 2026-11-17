using System.Text;
using Ileto;

namespace Ileto.Took;

internal sealed class AstPrinter : Expr.IVisitor<string>
{
    public string Print(Expr expr)
    {
        return expr.Accept(this);
    }

    public string VisitBinaryExpr(Expr.Binary expr)
    {
        return Parenthesize(expr.Operator.lexeme, expr.Left, expr.Right);
    }

    public string VisitGroupingExpr(Expr.Grouping expr)
    {
        return Parenthesize("group", expr.Expression);
    }

    public string VisitLiteralExpr(Expr.Literal expr)
    {
        if (expr.Value is null) return "nil";
        if (expr.Value is bool value) return value ? "true" : "false";
        return expr.Value?.ToString() ?? "nil";
    }

    public string VisitUnaryExpr(Expr.Unary expr)
    {
        return Parenthesize(expr.Operator.lexeme, expr.Right);
    }

    private string Parenthesize(string name, params Expr[] exprs)
    {
        var builder = new StringBuilder();
        builder.Append('(').Append(name);

        foreach (Expr expr in exprs)
        {
            builder.Append(' ').Append(Print(expr));
        }

        return builder.Append(')').ToString();
    }


}