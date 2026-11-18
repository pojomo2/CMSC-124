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
}