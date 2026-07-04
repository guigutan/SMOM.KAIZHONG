using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 推式调度获取工单bom
    /// </summary>
    [Serializable]
    public class WoBomPushPreInfo
    {
        /// <summary>
        /// bomId
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料扩展属性Id
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal BomNeedQty { get; set; }

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal SingleQty { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double ModelId { get; set; }

        /// <summary>
        /// 产品机型节拍
        /// </summary>
        public decimal? ProductModelMeter { get; set; }

        /// <summary>
        /// 产线节拍
        /// </summary>
        public decimal? ResourceMeter { get; set; }

        /// <summary>
        /// 工单占用可用数
        /// </summary>
        public decimal WoReportAvailableQty { get; set; }

        /// <summary>
        /// 产线线边仓
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 最高库存
        /// </summary>
        public decimal MaxStockQty { get; set; }
    }
}
