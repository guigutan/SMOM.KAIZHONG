using System;

namespace SIE.EventMessages.EMS.Repairs
{
    /// <summary>
    /// 设备维修工时和费用信息
    /// </summary>
    [Serializable]
    public class WorkHourAndCostInfo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId { get; set; }

        /// <summary>
        /// 维修工时
        /// </summary>
        public decimal RepairHours { get; set; }

        /// <summary>
        /// 保养工时
        /// </summary>
        public decimal MaintenanceHours { get; set; }

        /// <summary>
        /// 备件成本
        /// </summary>
        public decimal SparePartCost { get; set; }

        /// <summary>
        /// 委外维修成本
        /// </summary>
        public decimal OutRepairCost { get; set; }

        /// <summary>
        /// 累计维修工时
        /// </summary>
        public decimal TotalRepairHours { get; set; }

        /// <summary>
        /// 累计维修成本
        /// </summary>
        public decimal TotalSparePartCost { get; set; }
    }
}
