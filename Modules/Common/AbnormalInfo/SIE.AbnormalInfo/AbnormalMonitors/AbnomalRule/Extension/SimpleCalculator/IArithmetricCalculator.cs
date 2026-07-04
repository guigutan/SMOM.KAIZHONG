namespace SIE.AbnormalInfo.AbnormalMonitors.SimpleCalculator
{
    public interface IArithmetricCalculator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        double Calculate();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
         bool TryCalculate(out double result);
    }
}
