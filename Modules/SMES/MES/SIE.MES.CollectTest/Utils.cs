namespace SIE.MES.CollectTest
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// 转为int
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(string input, int defaultValue)
        {
            if (!int.TryParse(input?.Trim(), out int result))
                return defaultValue;
            return result;
        }
    }
}
