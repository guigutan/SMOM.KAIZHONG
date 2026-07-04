using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 设备维修工时查询结果 信息实体
    /// </summary>
    [Serializable]
    public class RepairWorkingHoursResultInfo
    {
        /// <summary>
        /// 维序工时ID
        /// </summary>
        public double RepairWorkingHourId { get; set; }

        /// <summary>
        /// 维修人ID
        /// </summary>
        public double? RepairerId { get; set; }

        /// <summary>
        /// 维修人编码
        /// </summary>
        public string RepairerCode { get; set; }

        /// <summary>
        /// 维修人名称
        /// </summary>
        public string RepairerName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否维修责任人
        /// </summary>
        public bool IsRepairMaster { get; set; }

        /// <summary>
        /// 是否当前维修人
        /// </summary>
        public bool IsRepairEmployee { get; set; }
    }
}
