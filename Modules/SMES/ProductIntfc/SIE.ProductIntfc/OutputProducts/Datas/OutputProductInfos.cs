using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ProductIntfc.OutputProducts.Datas
{
    /// <summary>
    /// 入库工单信息
    /// </summary>
    [Serializable]
    public class OutputProductInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>

        public decimal PlanQty { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 副产品信息
        /// </summary>
        public List<OutputProductDetailInfo> DetailInfos { get; set; } = new List<OutputProductDetailInfo>();


    }
    
    /// <summary>
    /// 副产品信息
    /// </summary>
    [Serializable]
    public class OutputProductDetailInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 副产品Id
        /// </summary>
        public double ItemId { get; set; }
        /// <summary>
        /// 副产品编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 副产品名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string ItemUnitName { get; set; }

        /// <summary>
        /// 需求量
        /// </summary>

        public decimal Qty { get; set; }

        /// <summary>
        /// 已报数量
        /// </summary>

        public decimal SubmitQty { get; set; }

        /// <summary>
        /// 副产品输入重量
        /// </summary>

        public decimal InputQty { get; set; }

    }
}
