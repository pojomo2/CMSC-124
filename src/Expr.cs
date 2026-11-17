using Ileto;
namespace Ileto.Took;

public abstract class Expr
{
      public interface IVisitor<R>
      {
      R VisitBinaryExpr(Binary expr);
      R VisitGroupingExpr(Grouping expr);
      R VisitLiteralExpr(Literal expr);
      R VisitUnaryExpr(Unary expr);
      }
     public sealed class Binary : Expr
   {
        public Expr Left { get; }
        public Token Operator { get; }
        public Expr Right { get; }

        public Binary(Expr left, Token @operator, Expr right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }
     public sealed class Grouping : Expr
   {
        public Expr Expression { get; }

        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }
     public sealed class Literal : Expr
   {
        public Object Value { get; }

        public Literal(object value)
        {
            Value = value;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
     public sealed class Unary : Expr
   {
        public Token Operator { get; }
        public Expr Right { get; }

        public Unary(Token @operator, Expr right)
        {
            Operator = @operator;
            Right = right;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }

      public abstract R Accept<R>(IVisitor<R> visitor);
}
