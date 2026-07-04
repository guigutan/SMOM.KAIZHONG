using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ShiftSchedules.Models
{
    /// <summary>
    /// 排班信息
    /// </summary>
    [Serializable]
    public class ScheduleData
    {
        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double WipResourceId { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 排班周期
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        /// 班组ID
        /// </summary>
        public double WorkGroupId { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 班次配置
        /// </summary>
        public Dictionary<double, string> ShiftConfig { get; set; } = new Dictionary<double, string>();
    }
}