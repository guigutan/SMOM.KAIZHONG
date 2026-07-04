using SIE.Core.Enums;
using SIE.EMS.Enums;
using SIE.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Common.ApiModels
{
    /// <summary>
    /// 离线盘点任务数据
    /// </summary>
    [Serializable]
    public class OffLineTaskBaseInfo
    {
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 盘点计划ID
        /// </summary>
        public double PlanId { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }

        /// <summary>
        /// 盘点类型
        /// </summary>
        public string InventoryType { get; set; }

        /// <summary>
        /// 盘点说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 资产对象
        /// </summary>
        public InventoryAssetObject InventoryAssetObject { get; set; }

        /// <summary>
        /// 资产对象名称
        /// </summary>
        public string InventoryAssetObjectName { get; set; }

        /// <summary>
        /// 管理部门ID
        /// </summary>
        public double? ManageDeptId { get; set; }

        /// <summary>
        /// 管理部门编码
        /// </summary>
        public string ManageDeptCode { get; set; }

        /// <summary>
        /// 管理部门名称
        /// </summary>
        public string ManageDeptName { get; set; }

        /// <summary>
        /// 使用部门ID
        /// </summary>
        public double? UseDeptId { get; set; }

        /// <summary>
        /// 使用部门编码
        /// </summary>
        public string UseDeptCode { get; set; }

        /// <summary>
        /// 使用部门名称
        /// </summary>
        public string UseDeptName { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypes { get; set; }

        /// <summary>
        /// 工治具类型ID集合
        /// </summary>
        public string FixtureTypeIds { get; set; }

        /// <summary>
        /// 工治具型号
        /// </summary>
        public string FixtureModels { get; set; }

        /// <summary>
        /// 工治具型号(ID集合)
        /// </summary>
        public string FixtureModelIds { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodes { get; set; }

        /// <summary>
        /// 工治具编码(ID集合)
        /// </summary>
        public string FixtureEncodeIds { get; set; }
    }

    /// <summary>
    /// 离线盘点设备清单
    /// </summary>
    [Serializable]
    public class EqpTaskList
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 盘点单ID
        /// </summary>
        public double InvTaskId { get; set; }

        /// <summary>
        /// 设备清单Id
        /// </summary>
        public double EqpId { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState? AccountState { get; set; }

        /// <summary>
        /// 设备状态名称
        /// </summary>
        public string AccountStateName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 使用责任人ID
        /// </summary>
        public double? UserId { get; set; }

        /// <summary>
        /// 使用责任人编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 使用责任人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 管理人状态
        /// </summary>
        public AccountUseState? AccountUseState { get; set; }

        /// <summary>
        /// 管理人状态名称
        /// </summary>
        public string AccountUseStateName { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? RealWorkShopId { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string RealWorkShopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string RealWorkShopName { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public double? RealResourceId { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string RealResourceCode { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string RealResourceName { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double? RealWarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string RealWarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string RealWarehouseName { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string RealLocation { get; set; }

        /// <summary>
        /// RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public string FileExtesion { get; set; }

        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryStatus InventoryStatus { get; set; }

        /// <summary>
        /// 盘点状态名称
        /// </summary>
        public string InventoryStatusName { get; set; }

        /// <summary>
        /// 管理部门ID
        /// </summary>
        public double? RealManageDeptId { get; set; }

        /// <summary>
        /// 管理部门名称
        /// </summary>
        public string RealManageDeptName { get; set; }

        /// <summary>
        /// 使用部门ID
        /// </summary>
        public double? RealUseDeptId { get; set; }

        /// <summary>
        /// 使用部门名称
        /// </summary>
        public string RealUseDeptName { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string PictureFileName { get; set; }

        /// <summary>
        /// 图片内容(base64)
        /// </summary>
        public string Picture { get; set; }
    }

    /// <summary>
    /// 离线盘点设备任务数据
    /// </summary>
    [Serializable]
    public class OffLineEqpTaskInfo : OffLineTaskBaseInfo
    {
        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryTaskStatus InventoryTaskStatus { get; set; }

        /// <summary>
        /// 盘点状态名称
        /// </summary>
        public string InventoryTaskStatusName { get; set; }

        /// <summary>
        /// 是否超期
        /// </summary>
        public bool IsOverdue { get; set; }

        /// <summary>
        /// 是否强制拍照
        /// </summary>
        public bool NeedPhoto { get; set; }

        /// <summary>
        /// 设备清单
        /// </summary>
        public List<EqpTaskList> EqpTaskLists { get; set; } = new List<EqpTaskList>();
    }

    /// <summary>
    /// 工治具盘点下载数据
    /// </summary>
    public class InvFixtureTaskList : OffLineTaskBaseInfo
    {
        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryTaskStatus InventoryTaskStatus { get; set; }

        /// <summary>
        /// 盘点状态名称
        /// </summary>
        public string InventoryTaskStatusName { get; set; }

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode? ManageMode { get; set; }

        /// <summary>
        /// 是否超期
        /// </summary>
        public bool IsOverdue { get; set; }
        /// <summary>
        /// 编码明细/序列号明细列表
        /// </summary>
        public List<InvFixtureDtlList> InvFixtureDtlTaskList { get; set; }
    }

    /// <summary>
    /// 工治具盘点下载数据
    /// </summary>
    [Serializable]
    public class InvFixtureAllData
    {
        /// <summary>
        /// 工治具数据
        /// </summary>
        public List<InvFixtureTaskList> InvFixtureTaskLists { get; set; } = new List<InvFixtureTaskList> { };

        /// <summary>
        /// 附件列表
        /// </summary>
        public List<FixtureFile> InvFixtureFile { get; set; } = new List<FixtureFile> { };
    }

    /// <summary>
    /// 工治具附件
    /// </summary>
    [Serializable]
    public class FixtureFile
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 附件后缀
        /// </summary>
        public string FileExtesion { get; set; }
    }

    /// <summary>
    /// 工治具提交数据
    /// </summary>
    [Serializable]
    public class SubmitFixtureTaskData : InvFixtureDtlList
    {
        /// <summary>
        /// 前端数据库单据ID
        /// </summary>
        public double ID { get; set; }

        /// <summary>
        /// 盘点结果
        /// </summary>
        public InventoryResult InventoryResult { get; set; }

        /// <summary>
        /// 实盘在库数
        /// </summary>
        public int RealStockQty { get; set; }

        /// <summary>
        /// 实盘在线数
        /// </summary>
        public int RealOnlineQty { get; set; }

        /// <summary>
        /// 盘点说明
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 工治具明细数据
    /// </summary>
    [Serializable]
    public class InvFixtureDtlList
    {
        /// <summary>
        /// 盘点单ID
        /// </summary>
        public double InvTaskId { get; set; }

        /// <summary>
        /// 编码明细ID
        /// </summary>
        public double FixtureEncodeId { get; set; }

        /// <summary>
        /// 序列号明细ID
        /// </summary>
        public double FixtureSnId { get; set; }

        /// <summary>
        /// 在库数
        /// </summary>
        public int? StockQty { get; set; }

        /// <summary>
        /// 在线数
        /// </summary>
        public int Online { get; set; }

        /// <summary>
        /// 在线数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode ManageMode { get; set; }

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode { get; set; }

        /// <summary>
        /// 工治具状态
        /// </summary>
        public FixtureStatus FixtureStatus { get; set; }

        /// <summary>
        /// 软件版本号
        /// </summary>
        public string SoftVersionNo { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidTime { get; set; }

        /// <summary>
        /// 盘点单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 工治具型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 工治具型号编码
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string RealLocation { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// FileExtesion
        /// </summary>
        public string FileExtesion { get; set; }
    }

    /// <summary>
    /// 盘点任务提交数据
    /// </summary>
    [Serializable]
    public class SubmitEqpTaskList
    {
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 盘点明细
        /// </summary>
        public List<SubmitDtlEqp> SubmitDtlList { get; set; }
    }

    /// <summary>
    /// 工治具盘点任务提交数据
    /// </summary>
    [Serializable]
    public class SubmitJigsTaskList
    {
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 盘点明细
        /// </summary>
        public List<SubmitDtlEqp> SubmitDtlList { get; set; }
    }

    /// <summary>
    /// 提交明细
    /// </summary>
    [Serializable]
    public class SubmitDtlEqp : EqpTaskList
    {
        /// <summary>
        /// 盘点结果
        /// </summary>
        public InventoryResult InventoryResult { get; set; }

        /// <summary>
        /// 盘点说明
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 位置快码数据
    /// </summary>
    public class LocationFastCode
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
