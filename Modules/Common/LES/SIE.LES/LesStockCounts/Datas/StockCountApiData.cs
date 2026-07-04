using SIE.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.LesStockCounts.Datas
{
    /// <summary>
    /// 盘点单列表
    /// </summary>
    public class StockCountList
    {
        /// <summary>
        /// 盘点单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 盘点单ID
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 盘点细度
        /// </summary>
        public CountDimension CountDimension { get; set; }

        /// <summary>
        /// 是否盲盘
        /// </summary>
        public bool IsBlindCount { get; set; }

        /// <summary>
        /// 是否动态盘点
        /// </summary>
        public bool IsDynamicOnhand { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 单据类型显示名称
        /// </summary>
        public string OrderTypeStr { get; set; }

        /// <summary>
        /// 审核时间显示
        /// </summary>
        public string AuditTimeStr { get; set; }
    }

    /// <summary>
    /// 盘点单仓库数据分组数据
    /// </summary>
    public class StockWareHouseList
    {
        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WareHouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }

        /// <summary>
        /// 已盘点数量
        /// </summary>
        public int FinishQty { get; set; }

        /// <summary>
        /// 未盘点数量
        /// </summary>
        public int UnFinishQty { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseName { get; set; }
    }

    /// <summary>
    /// 线边仓盘点盘点单数据
    /// </summary>
    public class StockDetailData
    {
        /// <summary>
        /// 盘点单ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal? OkQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal? NgQty { get; set; }

        /// <summary>
        /// 明细ID集合 线边仓盘点每次盘点可能会盘点2条明细 合格为一条 不合格为一条
        /// </summary>
        public List<double> DetailIds { get; set; }

        /// <summary>
        /// 明细行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WareHouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double? LocId { get; set; }

        /// <summary>
        /// 批次ID
        /// </summary>
        public double? LotId { get; set; }

        /// <summary>
        /// 可用数是否可填
        /// </summary>
        public bool HasOK { get; set; }

        /// <summary>
        /// NG数是否可填
        /// </summary>
        public bool HasNg { get; set; }

        /// <summary>
        /// 工厂ID
        /// </summary>
        public double? FactroyId{ get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }
    }

    /// <summary>
    /// 扫描识别数据
    /// </summary>
    public class ScanAutoData
    {
        /// <summary>
        /// 扫描类型 0-库位 1-物料 2-批次 3-标签
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料编码名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性组合
        /// </summary>
        public List<string> ItemExtPropList { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 批次号ID
        /// </summary>
        public double? LotId { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double? LocId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocCode { get; set; }

        /// <summary>
        /// 标签条码
        /// </summary>
        public string LabelNo { get; set; }
    }

    /// <summary>
    /// 盘点单任务数据
    /// </summary>
    public class StockGroupData
    {
        /// <summary>
        /// 待盘点数据
        /// </summary>
        public List<LesStockCountDetail> CountList { get; set; } = new List<LesStockCountDetail>();

        /// <summary>
        /// 已盘点数据
        /// </summary>
        public List<LesStockCountDetail> FinCountList { get; set; } = new List<LesStockCountDetail>();
    }
}
