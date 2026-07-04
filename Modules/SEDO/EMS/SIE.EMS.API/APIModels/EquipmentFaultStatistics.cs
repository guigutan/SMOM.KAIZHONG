using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.API.APIModels
{
    /// <summary>
    /// 设备故障统计
    /// </summary>
    [Serializable]
    public class EquipmentFaultStatistics
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 故障次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 维修时长(小时)
        /// </summary>
        public double RepairTimeTotal { get; set; }

        /// <summary>
        /// 设备故障总时长(小时)
        /// </summary>
        public double EquipMentFailureTime { get; set; }

        /// <summary>
        /// 设备运行时长(小时)
        /// </summary>
        public double RunningTime { get; set; }

        /// <summary>
        /// 平均无故障工作时间(小时)
        /// </summary>
        public double Mtbf { get; set; }

        /// <summary>
        /// 平均修复时间(分钟)
        /// </summary>
        public double Mttr { get; set; }
    }
}
