using DevExpress.XtraScheduler;
using System;

namespace SIE.Wpf.Resources.WipResources.Controls
{
    /// <summary>
    /// 日期显示
    /// </summary>
    public class CustomTimeScaleDay : TimeScaleDay
    {
        /// <summary>
        /// 格式化日期标题
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>日期</returns>
        public override string FormatCaption(DateTime start, DateTime end)
        {
            return string.Format("{0} {1}", start.ToString("M"), start.ToString("ddd"));
        }
    }
}