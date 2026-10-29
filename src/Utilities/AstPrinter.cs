using System;


using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

namespace Ileto.Took;


class AstPrinter : Expr.IVisitor<string>
{
    public string Print(Expression expr)
    {
        return expr.Accept<string>(this);
    }

    public string VisitBinaryExpr(Expr.Binary expr)
    {
        return Parenthesize(Expr.Operator.lexem, Expr.left, Expr.right);
    }

    public string VisitGroupingExpr(Expr.Grouping expr)
    {
        return Parenthesize("group", Expr.Expression);
    }

    
}