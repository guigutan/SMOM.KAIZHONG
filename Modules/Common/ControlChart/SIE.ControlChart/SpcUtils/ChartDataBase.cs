using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 控制图数据基类
    /// </summary>
    [Serializable]
    public class ChartDataBase
    {
        /// <summary>
        /// 数据计算精确度
        /// </summary>
        public int Digits { get; set; }
        /// <summary>
        /// 计算最大精度
        /// </summary>
        public static int GetMaxDigits(List<object> numbers)
        {
            int maxDigits = 0;
            numbers.ForEach(d =>
            {
                maxDigits = Math.Max(maxDigits, GetDigits(d));
            });

            //maxDigits += 1;
            if (maxDigits > 8)
                maxDigits = 8;
            return maxDigits;
        }

        /// <summary>
        /// 计算最大精度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetDigits(object value)
        {
            if (value == null)
                return 0;
            else
            {
                string strValue = Convert.ToDouble(value).ToString();
                if (strValue.IndexOf(".") == -1)
                    return 0;
                else
                {
                    return strValue.Length - strValue.IndexOf(".") - 1;
                }
            }
        }


        #region double类型计算最大精度
        /// <summary>
        /// 计算最大精度
        /// </summary>
        public static int GetMaxDigitsDouble(List<double> numbers)
        {
            int maxDigits = 0;
            numbers.ForEach(d =>
            {
                maxDigits = Math.Max(maxDigits, GetDigitsDouble(d));
            });

            //maxDigits += 1;
            if (maxDigits > 8)
                maxDigits = 8;
            return maxDigits;
        }

        /// <summary>
        /// 计算最大精度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetDigitsDouble(double value)
        {
            string strValue = value.ToString();
            if (strValue.IndexOf(".") == -1)
                return 0;
            else
            {
                return strValue.Length - strValue.IndexOf(".") - 1;
            }
        }
        #endregion

        /// <summary>
        /// 实现小数的向上取整（保留精度）
        /// </summary>
        /// <param name="v">要进行处理的数据</param>
        /// <param name="digit">保留的小数位数</param>
        /// <returns>小数的向上取整后的结果</returns>
        public static decimal Ceiling(decimal v, int digit)
        {
            int digitV = GetDigits(v);

            decimal? result = null;

            if (digitV > digit)
            {
                string strValue = v.ToString();
                string subStr = strValue.Substring(0, strValue.IndexOf(".") + 1 + digit);
                result = Convert.ToDecimal(subStr);
            }
            else
            {
                result = v;
            }

            return result.Value;
        }

        /// <summary>
        /// 获取精度的最小值
        /// </summary>
        /// <param name="digit">精度</param>
        /// <returns>如精度1返回0.1 精度2返回0.01</returns>
        public static decimal GetDigitMinValue(int digit)
        {
            if (digit < 0)
                throw new ArgumentException("不能小于0".L10N(), nameof(digit));
            if (digit == 0)
                return 1;

            StringBuilder s = new StringBuilder("0.");
            for (int i = 1; i <= digit; i++)
            {
                if (i == digit)
                {
                    s.Append("1");
                }
                else
                    s.Append(0);
            }
            decimal result = Convert.ToDecimal(s.ToString());
            return result;
        }
    }
}
