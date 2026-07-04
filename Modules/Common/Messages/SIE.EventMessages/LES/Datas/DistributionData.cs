using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 配送数据
    /// </summary>
    [Serializable]
    public class DistributionData
    {
        /// <summary>
        /// 来源单据号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 来源仓库Id
        /// </summary>
        public double SourceWhId { get; set; }

        /// <summary>
        /// 来源库位
        /// </summary>
        public double SourceLocId { get; set; }

        /// <summary>
        /// 是否工单发料
        /// </summary>
        public bool IsWorkFeed { get; set; }

        /// <summary>
        /// 明细数据
        /// </summary>
        public List<DistributionDtlData> DistributionDtls { get; set; } = new List<DistributionDtlData>();

        /// <summary>
        /// 扫描记录
        /// </summary>
        public List<SoLabelData> SoLabels { get; set; } = new List<SoLabelData>();
    }

    [Serializable]
    public class DistributionDtlData
    {
        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 分配Id
        /// </summary>
        public string AssignId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public int OnhandState { get; set; }

        /// <summary>
        /// 本次发货数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo { get; set; }

        /// <summary>
        /// 发运单明细Id
        /// </summary>
        public double SoDtlId { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 相关单行号
        /// </summary>
        public string OrderLineNo { get; set; }
    }
}
