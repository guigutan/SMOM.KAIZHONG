using SIE.Common.Algorithm;
using System;
using System.Globalization;

namespace SIE.Core.Algorithms.KZ
{
    /// <summary>
    /// 大众生产日期-周月日算法
    /// </summary>
    [Algorithm("大众生产日期-周月日算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class DateVWWeekMonthDaySegmentAlgorithm : EntityCodeAlgorithm
    {
        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public override string GetCode()
        {
            var date = DateTime.Now;
            //②.周次（两位，01-53）
            GregorianCalendar cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
            int weekOfYear = cal.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var weekCode = weekOfYear < 10 ? "0" + weekOfYear : weekOfYear.ToString();
            //③.月份（两位，01-12）
            var monthCode = date.ToString("MM");
            //④.日期（两位，01-31）
            var dayCode = date.ToString("dd");

            return "{0}{1}{2}".FormatArgs(weekCode, monthCode, dayCode);
        }
    }
}
