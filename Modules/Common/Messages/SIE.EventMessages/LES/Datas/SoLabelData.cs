using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 发运单序列号、标签数据
    /// </summary>
    [Serializable]
    public class SoLabelData
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 最上级条码
        /// </summary>
        public string HighestNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// AssignId
        /// </summary>
        public string AssignId { get; set; }

        /// <summary>
        /// 是否序列号管理
        /// </summary>
        public bool IsSerialNumber { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 相关单行号
        /// </summary>
        public string OrderLineNo { get; set; }

        /// <summary>
        /// 是否工单发料
        /// </summary>
        public bool IsWorkFeed { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 批次管理
        /// </summary>
        public bool IsBatch { get; set; }

        /// <summary>
        /// 标签查询Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 配送单号
        /// </summary>
        public string DistributionNo { get; set; }
    }
}
