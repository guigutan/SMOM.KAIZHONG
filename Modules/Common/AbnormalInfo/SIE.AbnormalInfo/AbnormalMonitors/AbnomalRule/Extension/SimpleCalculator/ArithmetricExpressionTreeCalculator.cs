using System;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;

namespace SIE.AbnormalInfo.AbnormalMonitors.SimpleCalculator
{
    public class ExpressionTreeArithmetricCalculator : IArithmetricCalculator
    {
        private static ExpressionTreeArithmetricCalculator _calculator;

        private Expression _expr;
        
        internal IExpressionTreeCalculatorEngine CalculateEngine { get; set; }

        private ExpressionTreeArithmetricCalculator(Expression expr)
        {
            SetExpr(expr);
            CalculateEngine = new DefaultCalculatorEngine();
        }

        private void SetExpr(Expression expr)
        {
            _expr=expr ?? throw new ArgumentNullException(nameof(expr));
        }

        public double Calculate()
        {
            return CalculateEngine.Calculate(_expr);
        }

        public static IArithmetricCalculator Create(Expression expr)
        {
            if (_calculator == null) _calculator = new ExpressionTreeArithmetricCalculator(expr);

            else _calculator.SetExpr(expr);

            return _calculator;
        }

        public static IArithmetricCalculator Create(string exprText)
        {
            try
            {
                BinaryExpression expression = new ArithmetricExpressionBuilder(exprText).Build();
                return Create(expression);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                throw new InvalidExpressionException("Fatal error, unsupported syntax, please check user input!", ex);
            }
        }

        public bool TryCalculate(out double result)
        {
            try
            {
                result = Calculate();
                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                result = 0;
                return false;
            }
        }
    }
}
