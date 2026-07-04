using System;

namespace SIE.Kit.QMS.ApiModels.InspBoard
{
    /// <summary>
    /// 当天检验超时情况
    /// </summary>
    [Serializable]
    public class InspDayOvertimeInfo
    {
        /// <summary>
        /// 小时
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// 超时检验单据数量
        /// </summary>
        public int OvertimeQty { get; set; }

        /// <summary>
        /// 正常检验单据数量
        /// </summary>
        public int NormalQty { get; set; }
    }
}
