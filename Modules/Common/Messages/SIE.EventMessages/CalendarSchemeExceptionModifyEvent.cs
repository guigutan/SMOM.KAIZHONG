using System;

namespace SIE.EventMessages
{
    /// <summary>
    /// 日历方案例外
    /// </summary>
    public class CalendarSchemeExceptionModifyEvent
    {
        /// <summary>
        /// 例外日期
        /// </summary>
        public DateTime CalendarDay { get; set; }

        /// <summary>
        /// 班制ID
        /// </summary>
        public double ShiftTypeId { get; set; }

        /// <summary>
        /// 日历方案ID
        /// </summary>
        public double SchemeId { get; set; }
    }
}
