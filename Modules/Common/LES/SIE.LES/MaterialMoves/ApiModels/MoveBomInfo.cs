using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialMoves.ApiModels
{
    /// <summary>
    /// 挪料bom信息
    /// </summary>
    [Serializable]
    public class MoveBomInfoWithCount
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 挪料bom信息
        /// </summary>
        public List<MoveBomInfo> BomInfos { get; set; } = new List<MoveBomInfo>();
    }


    /// <summary>
    /// 挪料bom信息
    /// </summary>
    [Serializable]
    public class MoveBomInfo
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料扩展属性Id
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 工单占用Id
        /// </summary>
        public double? WoDemandReportId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 可接收数
        /// </summary>
        public decimal ReceivedQty { get; set; }

        /// <summary>
        /// 挪入数
        /// </summary>
        public decimal MovedInQty { get; set; }

        /// <summary>
        /// 上料数
        /// </summary>
        public decimal FeedQty { get; set; }

        /// <summary>
        /// 不良数
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 挪出数
        /// </summary>
        public decimal MovedOutQty { get; set; }

        /// <summary>
        /// 正常退料在途数
        /// </summary>
        public decimal ReturnQtyInTransit { get; set; }

        /// <summary>
        /// 不良退料在途数
        /// </summary>
        public decimal NgReturnQtyInTransit { get; set; }

        /// <summary>
        /// 正常退料数
        /// </summary>
        public decimal ReturnQty { get; set; }

        /// <summary>
        /// 不良退料数
        /// </summary>
        public decimal NgReturnQty { get; set; }

        /// <summary>
        /// 可用数
        /// </summary>
        public decimal AvailableQty { get; set; }

        /// <summary>
        /// 挪料数
        /// </summary>
        public decimal MoveQty { get; set; }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
