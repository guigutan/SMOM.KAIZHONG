using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ShiftSchedules.Models
{
    /// <summary>
    /// 排班表信息
    /// </summary>
    [Serializable]
    public class ShiftScheduleInfo
    {
        /// <summary>
        /// 班组名称
        /// </summary>
        public string WorkGroup { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string WipResource { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShop { get; set; }

        /// <summary>
        /// 排班明细
        /// </summary>
        public List<ShiftScheduleDetailInfo> DetailList { get; set; } = new List<ShiftScheduleDetailInfo>();
    }

    /// <summary>
    /// 排班明细
    /// </summary>
    [Serializable]
    public class ShiftScheduleDetailInfo
    {
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 排班日期字符串
        /// </summary>
        public string StrDate { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 班次时间
        /// </summary>
        public string ShiftTime { get; set; }

        /// <summary>
        /// 班次背景色
        /// </summary>
        public string Background { get; set; }
    }
}