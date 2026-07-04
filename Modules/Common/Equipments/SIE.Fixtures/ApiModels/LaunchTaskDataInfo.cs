using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 分页上架任务信息
    /// </summary>
    [Serializable]
    public class LaunchTaskDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 上架任务信息列表
        /// </summary>
        public List<LaunchTaskInfo> LaunchTaskInfos { get; } = new List<LaunchTaskInfo>();
    }

    /// <summary>
    /// 上架任务信息
    /// </summary>
    [Serializable]
    public class LaunchTaskInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>

        public LaunchTaskInfo()
        {
            Details = new List<LaunchTaskInfoDetail>();
        }
        /// <summary>
        /// 上架任务号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 管理方式
        /// </summary>
        public string ManageMode { get; set; }

        /// <summary>
        /// 管理方式(值)
        /// </summary>
        public int ManageModeValue { get; set; }

        /// <summary>
        /// 固定储位
        /// </summary>
        public string FixedStorage { get; set; }

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
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? LocationId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 上架数量
        /// </summary>
        public int LaunchTaskQty { get; set; }

        /// <summary>
        /// 已入库数量
        /// </summary>
        public int InboundQty { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 保养状态
        /// </summary>
        public string MaintainStatus { get; set; }

        /// <summary>
        /// 入库明细
        /// </summary>
        public List<LaunchTaskInfoDetail> Details { get; set; }
    }

    /// <summary>
    /// 入库明细
    /// </summary>
    [Serializable]

    public class LaunchTaskInfoDetail
    {
        /// <summary>
        /// 数据库对应的Id 编码类无Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 工治具ID/编码
        /// </summary>
        public string IDCode { get; set; }
        /// <summary>
        /// RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType { get; set; }
        /// <summary>
        /// 本次数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocaltionId { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsSelect { get; set; }
    }
}
