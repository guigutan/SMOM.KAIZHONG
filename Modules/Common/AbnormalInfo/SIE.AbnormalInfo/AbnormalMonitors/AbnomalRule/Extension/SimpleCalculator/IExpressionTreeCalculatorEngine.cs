using System.Linq.Expressions;

namespace SIE.AbnormalInfo.AbnormalMonitors.SimpleCalculator
{
    internal interface IExpressionTreeCalculatorEngine
    {
        double Calculate(Expression expression);
    }
}
