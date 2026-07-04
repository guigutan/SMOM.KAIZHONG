using SIE.EMS.Enums;
using System;

namespace SIE.EMS.InventoryTasks.ApiModels
{
    /// <summary>
    /// 盘点任务信息
    /// </summary>
    [Serializable]
    public class InventoryTaskInfo
    {
        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 计划完成日期
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 盘点类型
        /// </summary>
        public string InventoryType { get; set; }

        /// <summary>
        /// 管理部门ID
        /// </summary>
        public double ManageDeptId { get; set; }

        /// <summary>
        /// 管理部门
        /// </summary>
        public string ManageDeptName { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDeptName { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 需盘点
        /// </summary>
        public int NeedInventory { get; set; }

        /// <summary>
        /// 未盘点
        /// </summary>
        public int NoInventory { get; set; }

        /// <summary>
        /// 盘点单的工厂ID
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 是否强制拍照
        /// </summary>
        public bool NeedPhoto { get; set; }

        /// <summary>
        /// 盘点说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否超期
        /// </summary>
        public bool IsOverdue { get; set; }

        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryTaskStatus InventoryTaskStatus { get; set; }

        /// <summary>
        /// 盘点状态名称
        /// </summary>
        public string InventoryTaskStatusName { get; set; }

        /// <summary>
        /// 图片内容
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public string Progress { get; set; }
    }

    /// <summary>
    /// 工治具盘点
    /// </summary>
    [Serializable]
    public class FixtureTaskInfo: InventoryTaskInfo
    {
        /// <summary>
        /// 公治具类型
        /// </summary>
        public string FixtureTypes { get; set; }

        /// <summary>
        /// 公治具编码
        /// </summary>
        public string FixtureEncodes { get; set; }

        /// <summary>
        /// 公治具型号
        /// </summary>
        public string FixtureModels { get; set; }


        /// <summary>
        /// 盘点进度
        /// </summary>
        public decimal Percentage { get; set; }

        /// <summary>
        /// 执行类型
        /// </summary>
        public InventoryExecuteType InventoryExecuteType { get; set; }
    }

}
