using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace SIE.AbnormalInfo.AbnormalMonitors.SimpleCalculator
{
    internal sealed class RecursionExpressionTreeCalculatorEngine : ExpressionVisitor,IExpressionTreeCalculatorEngine
    {

        public double Calculate(Expression expression)
        {
            Expression exp = Visit(expression);
            var constant = exp as ConstantExpression;
            return (double)constant.Value;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression l = Visit(node.Left);
            Expression r = Visit(node.Right);
            
            if (l is ConstantExpression cl && r is ConstantExpression cr)
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        Debug.Write($"(+{cl.Value} {cr.Value})");
                        return Expression.Constant((double)cl.Value+(double)cr.Value);
                    case ExpressionType.Divide:
                        Debug.Write($"(/{cl.Value} {cr.Value})");
                        return Expression.Constant((double)cl.Value/(double)cr.Value);
                    case ExpressionType.Subtract:
                        Debug.Write($"(-{cl.Value} {cr.Value})");
                        return Expression.Constant((double)cl.Value-(double)cr.Value);
                    case ExpressionType.Multiply:
                        Debug.Write($"(*{cl.Value} {cr.Value})");
                        return Expression.Constant((double)cl.Value*(double)cr.Value);
                    case ExpressionType.PowerAssign:
                    case ExpressionType.Power:
                        Debug.Write($"(^{cl.Value} {cr.Value})");
                        return Expression.Constant(Math.Pow((double)cl.Value, (double)cr.Value));
                    case ExpressionType.Modulo:
                        Debug.Write($"(/{cl.Value} {cr.Value})");
                        return Expression.Constant((double)cl.Value%(double)cr.Value);
                    //case ExpressionType
                    default:
                        throw new NotSupportedException();
                }
            else
            {
                Debug.Write(node.ToString());
                return node;
            }
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return node;
        }
    }
}
