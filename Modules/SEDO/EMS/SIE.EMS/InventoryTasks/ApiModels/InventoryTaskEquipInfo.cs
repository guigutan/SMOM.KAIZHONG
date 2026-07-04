using SIE.EMS.Enums;
using System;

namespace SIE.EMS.InventoryTasks.ApiModels
{
    /// <summary>
    /// 盘点任务设备清单信息
    /// </summary>
    [Serializable]
    public class InventoryTaskEquipInfo
    {
        /// <summary>
        /// 设备清单id
        /// </summary>
        public double InventoryTaskEquipmentId { get; set; }

        /// <summary>
        /// 盘点结果
        /// </summary>
        public string InventoryResult { get; set; }

        /// <summary>
        /// 盘点结果值
        /// </summary>
        public int InventoryResultValue { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 是否完成盘点（用于区分待盘点、已盘点）
        /// </summary>
        public bool IsFinishInventory { get; set; }
    }

    /// <summary>
    /// 盘点任务设备清单更多信息
    /// </summary>
    [Serializable]
    public class InventoryTaskEquipMoreInfo
    {
        /// <summary>
        /// 盘点结果
        /// </summary>
        public string InventoryResult { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 使用部门名称
        /// </summary>
        public string UseDeptName { get; set; }

        /// <summary>
        /// 使用责任人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 管理状态
        /// </summary>
        public string AccountUseStateDisplay { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public string AccountStateDisplay { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }
    }
}
