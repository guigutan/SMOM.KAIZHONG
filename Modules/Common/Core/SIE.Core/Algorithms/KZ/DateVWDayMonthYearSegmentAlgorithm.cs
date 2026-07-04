using SIE.Common.Algorithm;
using System;

namespace SIE.Core.Algorithms.KZ
{
    /// <summary>
    /// 大众生产日期-日月年算法
    /// </summary>
    [Algorithm("大众生产日期-日月年算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class DateVWDayMonthYearSegmentAlgorithm : EntityCodeAlgorithm
    {
        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public override string GetCode()
        {
            var date = DateTime.Now;
            //②.日期（两位，01-31）
            var dayCode = date.ToString("dd");
            //③.月份（两位，01-12）
            var monthCode = date.ToString("MM");
            //④.年份（两位，如26）
            var yearCode = date.ToString("yy");

            return "{0}{1}{2}".FormatArgs(dayCode, monthCode, yearCode);
        }
    }
}
