using SIE.Domain;
using SIE.EventMessages.WMS.Traces;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 过程追溯-关联产品追溯
    /// </summary>
    [Serializable]
    public class TraceInfoForProduct
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<TraceItemInfoForProduct> Data { get; set; } = new List<TraceItemInfoForProduct>();
    }

    /// <summary>
    /// 过程追溯-关联产品追溯详细信息
    /// </summary>
    [Serializable]
    public class TraceItemInfoForProduct
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// treeid
        /// </summary>
        public string TreePId { get; set; }

        /// <summary>
        /// 关键件Id
        /// </summary>
        public double WipProductKeyItemId { get; set; }

        /// <summary>
        /// 生产通用报表
        /// </summary>
        public double WipProductVersionId { get; set; }

        /// <summary>
        /// 关联产品Id
        /// </summary>
        public double RelatedProductId { get; set; }

        /// <summary>
        /// 关联产品条码
        /// </summary>
        public string RelatedProductSn { get; set; }

        /// <summary>
        /// 关联产品批次
        /// </summary>
        public string RelatedProductLot { get; set; }

        /// <summary>
        /// 关联产品编码
        /// </summary>
        public string RelatedProductCode { get; set; }

        /// <summary>
        /// 关联产品名称
        /// </summary>
        public string RelatedProductName { get; set; }

        /// <summary>
        /// 关联工单号
        /// </summary>
        public string RelatedWorkOrderNo { get; set; }

        /// <summary>
        /// 关联工单Id
        /// </summary>
        public double RelatedWorkOrderId { get; set; }


        /// <summary>
        /// 产品扩展属性
        /// </summary>
        public string ProductExtProp { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料来源码
        /// </summary>
        public string ItemSourceCode { get; set; }

    }
}
