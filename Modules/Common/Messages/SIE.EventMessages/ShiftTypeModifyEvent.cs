using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 班制修改事件参数
    /// </summary>
    public class ShiftTypeModifyEvent
    {
        /// <summary>
        /// 班制ID列表
        /// </summary>
        public List<double> ShiteTypeIds { get; set; }
    }
}
