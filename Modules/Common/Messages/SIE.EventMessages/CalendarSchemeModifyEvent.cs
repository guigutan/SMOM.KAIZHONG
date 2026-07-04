using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 日历方案修改事件参数
    /// </summary>
    public class CalendarSchemeModifyEvent
    {
        /// <summary>
        /// 日历方案ID列表
        /// </summary>
        public List<double> CalendarSchemeIds { get; set; }
    }
}
