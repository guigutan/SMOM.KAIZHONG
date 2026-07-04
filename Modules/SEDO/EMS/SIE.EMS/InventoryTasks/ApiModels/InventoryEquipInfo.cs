using SIE.Core.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using System;

namespace SIE.EMS.InventoryTasks.ApiModels
{
    /// <summary>
    /// 盘点任务设备信息
    /// </summary>
    [Serializable]
    public class InventoryEquipInfo
    {
        /// <summary>
        /// 盘点PDA类型:1-盘点执行,2-复盘执行
        /// </summary>
        public int PdaType { get; set; }

        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 是否获取到设备台账
        /// </summary>
        public bool IsHaveEquip { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public double EquipId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory { get; set; }

        /// <summary>
        /// 设备类型id
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 设备型号id
        /// </summary>
        public double? EquipModelId { get; set; }

        /// <summary>
        /// ABC分类
        /// </summary>
        public string UseLevel { get; set; }

        /// <summary>
        /// 管理部门id
        /// </summary>
        public double? ManageDeptId { get; set; }

        /// <summary>
        /// 管理部门名称
        /// </summary>
        public string ManageDeptName { get; set; }

        /// <summary>
        /// 使用部门id
        /// </summary>
        public double? UseDeptId { get; set; }

        /// <summary>
        /// 使用部门名称
        /// </summary>
        public string UseDeptName { get; set; }

        /// <summary>
        /// 使用责任人id 
        /// </summary>
        public double? UserId { get; set; }

        /// <summary>
        /// 使用责任人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 管理状态
        /// </summary>
        public int? AccountUseState { get; set; }

        /// <summary>
        /// 管理状态
        /// </summary>
        public string AccountUseStateDisplay { get; set; }

        /// <summary>
        /// 设备状态值
        /// </summary>
        public int? AccountState { get; set; }

        /// <summary>
        /// 设备状态值
        /// </summary>
        public string AccountStateDisplay { get; set; }

        /// <summary>
        /// 车间id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 产线id
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库位id
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 图片文件名称
        /// </summary>
        public string PictureFileName { get; set; }

        /// <summary>
        /// 图片内容
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 工厂ID
        /// </summary>
        public double FactoryId { get; set; }
    }

    /// <summary>
    /// 公治具台账信息
    /// </summary>

    public class InventoryFixtureInfo
    {
        /// <summary>
        /// 盘点PDA类型:1-盘点执行,2-复盘执行
        /// </summary>
        public int PdaType { get; set; }

        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId { get; set; }

        /// <summary>
        /// 是否获取到公治具台账
        /// </summary>
        public bool IsHaveFixture { get; set; }

        /// <summary>
        /// 公治具编码
        /// </summary>
        public string EncodeCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 合格数/良品数
        /// </summary>
        public int PassQty { get; set; }

        /// <summary>
        /// 不合格数/不良数
        /// </summary>
        public int NgQty { get; set; }

        /// <summary>
        /// 在线数
        /// </summary>
        public int OnlineQty { get; set; }

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode { get; set; }

        /// <summary>
        /// 质量状态
        /// </summary>
        public FixtureQualityState? QualityState { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalQty { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public FixtureAccountState OnHandStatus{ get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn { get; set; }
    }

    /// <summary>
    /// 上传返回数据类
    /// </summary>
    [Serializable]
    public class UploadLoadReturnData
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 行号/ID
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailReason { get; set; }


    }
}
