using SIE.EMS.Enums;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务设备清单信息
    /// </summary>
    [Serializable]
    public class InventoryTaskEquipmentInfo
    {
        /// <summary>
        /// 计划id
        /// </summary>
        public double InventoryPlanId { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 资产来源
        /// </summary>
        public InventoryAssetSource InventoryAssetSource { get; set; }

        /// <summary>
        /// 初盘结果
        /// </summary>
        public InventoryResult? FirstInventoryResult { get; set; }
    }
}
