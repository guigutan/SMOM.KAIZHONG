using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 单工单出库工单信息
    /// </summary>
    [Serializable]
    public class UnloadWorkOrderInfo : ResultDataInfo
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public double WorkOrderId { get; set; }
    }

    /// <summary>
    ///  单工治具出库信息
    /// </summary>
    [Serializable]
    public class SingleUnloadInfo
    {
        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 仓位Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
    }

    /// <summary>
    /// 工单查询信息
    /// </summary>
    [Serializable]
    public class SingleWorkOrderQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId { get; set; }
    }

    /// <summary>
    /// 分页工单信息
    /// </summary>
    [Serializable]
    public class SingleWorkOrderDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 工单信息列表
        /// </summary>
        public List<SingleWorkOrderInfo> WorkOrderInfos { get; } = new List<SingleWorkOrderInfo>();
    }

    /// <summary>
    /// 工单信息
    /// </summary>
    [Serializable]
    public class SingleWorkOrderInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }
    }

    /// <summary>
    /// 单工治具库位查询信息
    /// </summary>
    [Serializable]
    public class SingleLocationQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode { get; set; }
    }

    /// <summary>
    /// 分页库位信息
    /// </summary>
    [Serializable]
    public class SingleLocationDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 库位信息列表
        /// </summary>
        public List<SingleLocationInfo> LocationInfos { get; } = new List<SingleLocationInfo>();
    }

    /// <summary>
    /// 库位信息
    /// </summary>
    [Serializable]
    public class SingleLocationInfo
    {
        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 库存合格数
        /// </summary>
        public int Qty { get; set; }
    }

    /// <summary>
    /// 工治具类型信息
    /// </summary>
    [Serializable]
    public class FixtureTypeInfo
    {
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        public double FixtureTypeId { get; set; }

        /// <summary>
        /// 工治具类型名称
        /// </summary>
        public string FixtureTypeName { get; set; }
    }

    /// <summary>
    /// 实际工治具仓库数据信息
    /// </summary>
    [Serializable]
    public class SingleActWarehouseInfo
    {
        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// 库位（在库数量）
        /// </summary>
        public string Location { get; set; }
    }

    /// <summary>
    /// 实际工治具仓库信息
    /// </summary>
    [Serializable]
    public class SingleActWarehouseDataInfo
    {
        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode { get; set; }

        /// <summary>
        /// 库位列表
        /// </summary>
        public List<SingleActLocationDataInfo> LocationDataInfos { get; } = new List<SingleActLocationDataInfo>();
    }

    /// <summary>
    /// 实际工治具库位信息
    /// </summary>
    [Serializable]
    public class SingleActLocationDataInfo
    {
        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位（在库数量）
        /// </summary>
        public string Location { get; set; }
    }
}

