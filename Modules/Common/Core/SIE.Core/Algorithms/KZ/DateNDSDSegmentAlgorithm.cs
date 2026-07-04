using SIE.Common.Algorithm;
using System;
using System.Globalization;

namespace SIE.Core.Algorithms.KZ
{

    /// <summary>
    /// KZ-宁德时代日期算法
    /// </summary>
    [Algorithm("KZ-宁德时代日期算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class DateNDSDSegmentAlgorithm : EntityCodeAlgorithm
    {
        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public override string GetCode()
        {
            var date = DateTime.Now;
            //③.年份
            var yearCode = date.ToString("yy");   
            //④.周次
            GregorianCalendar cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
            int weekOfYear = cal.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);  //         21 = ④.周次（25年第21周）
            //⑤.天(0-6,0表示周日)
            var weekIndex = (int)date.DayOfWeek;

            return "{0}{1}{2}".FormatArgs(yearCode, weekOfYear < 10 ? "0" + weekOfYear : weekOfYear, weekIndex);
        }

    }
}
