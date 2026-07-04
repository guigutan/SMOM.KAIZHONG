using System;

namespace SIE.ControlChart.Extension
{
    /// <summary>
    /// 基础类型转换扩展
    /// </summary>
    public static class LangExt
    {
        /// <summary>
        /// 将String转Int16类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static short ToInt16(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return 0;
            else return Convert.ToInt16(val);
        }
        /// <summary>
        /// 将String转Int32类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt32(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return 0;
            else return Convert.ToInt32(val);
        }
        /// <summary>
        /// 将String转Int64类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static long ToInt64(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return 0;
            else return Convert.ToInt64(val);
        }
        /// <summary>
        /// 将String转Float类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static float ToFloat(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return 0;
            else return Convert.ToSingle(val);
        }

        /// <summary>
        /// 将任何类型装箱为Object
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object ToObject(this object val)
        {
            if (val == null)
                return null;
            else return val;
        }

        /// <summary>
        /// 将String转Decimal类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return 0;
            else return Convert.ToDecimal(val);
        }

        /// <summary>
        /// 将Double转Int类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt(this double val)
        {
            return Convert.ToInt32(val);
        }
        /// <summary>
        /// 将String转Double类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double ToDouble(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return 0;
            else return Convert.ToDouble(val);
        }
        /// <summary>
        /// 将Int转Double类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double ToDouble(this int val)
        {
            return Convert.ToDouble(val);
        }
        /// <summary>
        /// 对Double值取N位小数（四舍五入）
        /// </summary>
        /// <param name="val"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double ToFixed(this double val, int digits)
        {
            //MidpointRounding.AwayFromZero 解决判断位为5，前一位为偶数时，不进一的问题
            return Math.Round(val, digits, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// 对Double?值取N位小数（四舍五入）
        /// </summary>
        /// <param name="val"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double? ToFixed(this double? val, int digits)
        {
            if (val.HasValue)
                return Math.Round(val.Value, digits, MidpointRounding.AwayFromZero); //MidpointRounding.AwayFromZero 解决判断位为5，前一位为偶数时，不进一的问题
            else
                return val;
        }
    }
}
