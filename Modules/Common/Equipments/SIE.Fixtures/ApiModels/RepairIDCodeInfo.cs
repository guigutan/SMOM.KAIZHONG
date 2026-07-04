using SIE.Core.ApiModels;
using SIE.Core.Enums;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Repairs.ViewModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 报修ID编码信息 
    /// </summary>
    [Serializable]
    public class RepairIDCodeInfo : ResultDataInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RepairIDCodeInfo()
        {
            this.FixtureRepairDetailInfo = new FixtureRepairDetailInfo();
        }

        /// <summary>
        /// 质量状态
        /// </summary>
        public int QualityState { get; set; }

        /// <summary>
        /// ID编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 治具编码
        /// </summary>
        public string EncodeCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType { get; set; }

        /// <summary>
        /// 管理方式
        /// </summary>
        public string ManageMode { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }


        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? LocationId { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>

        public string StorageLocationName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 报修时工治具的信息
        /// </summary>
        public FixtureRepairDetailInfo FixtureRepairDetailInfo { get; set; }
    }

    /// <summary>
    /// 异常现象查询信息
    /// </summary>
    [Serializable]
    public class AbnormalQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 工治具报修信息
    /// </summary>
    [Serializable]
    public class RepairInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>

        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 质量状态
        /// </summary>

        public FixtureQualityState? QualityState { get; set; }

        /// <summary>
        /// 报修前状态（在库/在线)
        /// </summary>
        public int RepairBeforeState { get; set; }

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 治具编码
        /// </summary>
        public string EncodeCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType { get; set; }

        /// <summary>
        /// 异常现象Id
        /// </summary>
        public double AbnormalId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? LocationId { get; set; }

        /// <summary>
        /// 上传图片文件信息列表
        /// </summary>
        public List<RepairFileInfo> RepairFileInfos { get; set; } = new List<RepairFileInfo>();

        /// <summary>
        /// 数量（在库、在线和使用中）
        /// </summary>
        public int Qty { get; set; }
    }

    /// <summary>
    /// 报修文件信息
    /// </summary>
    [Serializable]
    public class RepairFileInfo
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string DataURL { get; set; }
    }

    /// <summary>
    /// 分页异常现象信息
    /// </summary>
    [Serializable]
    public class AbnormalDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 异常现象信息集
        /// </summary>
        public List<AbnormalInfo> AbnormalInfos { get; } = new List<AbnormalInfo>();
    }

    /// <summary>
    /// 异常现象信息
    /// </summary>
    [Serializable]
    public class AbnormalInfo
    {
        /// <summary>
        /// 异常现象Id
        /// </summary>
        public double AbnormalId { get; set; }

        /// <summary>
        /// 异常现象编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 异常现象编码描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 异常现象编码名称(描述)
        /// </summary>
        public string AbnormalDescription { get; set; }
    }
}
