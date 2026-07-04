using SIE.Core.ApiModels;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.LES.LinesideWarehouses.Models;
using System;
using System.Collections.Generic;

namespace SIE.LES.MaterialReturnApplys.ApiModels
{
    /// <summary>
    /// 退料申请数据
    /// </summary>
    [Serializable]
    public class MaterialWoData
    {
        /// <summary>
        /// 工单信息
        /// </summary>
        public List<WorkOrderSimpleInfo> WorkOrderInfos { get; set; } = new List<WorkOrderSimpleInfo>();

        /// <summary>
        /// 车间信息
        /// </summary>
        public List<BaseDataInfo> WorkShopInfos { get; set; } = new List<BaseDataInfo>();

        /// <summary>
        /// 仓库信息
        /// </summary>
        public List<LinesideWareBaseData> WarehouseInfos { get; set; } = new List<LinesideWareBaseData>();

        /// <summary>
        /// 退料原因
        /// </summary>
        public List<BaseDataInfo> ResonInfos { get; set; } = new List<BaseDataInfo>();
    }

    /// <summary>
    /// 退料申请明细数据
    /// </summary>
    [Serializable]
    public class MaterialDtlData
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 明细数据
        /// </summary>
        public List<MaterialReturnApplyDetailSelect> DetailList { get; set; } = new List<MaterialReturnApplyDetailSelect>();
    }

    /// <summary>
    /// 退料信息主数据
    /// </summary>
    [Serializable]
    public class MaterialReturnApplyMainInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WoId { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 库位id
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectId { get; set; }

        /// <summary>
        /// 项目号编码
        /// </summary>
        public string ProjectNo { get; set; }
    }
}
