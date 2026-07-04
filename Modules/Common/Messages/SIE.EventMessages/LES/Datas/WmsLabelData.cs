using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 接收WMS标签数据
    /// </summary>
    [Serializable]
    public class ReciveWmsLabelData
    {
        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 分配明细Id
        /// </summary>
        public double SoAssignId { get; set; }

        /// <summary>
        /// 分配ID
        /// </summary>
        public string AssignId { get; set; }

        /// <summary>
        /// 返回标签数量（辅）发运单明细的辅助单位对应的数量, 多行需要汇总才是标签对应的数量
        /// </summary>
        public decimal SecondQty { get; set; }

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string SecondUnitName { get; set; }

        /// <summary>
        /// 相关单号/备料需求单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo { get; set; }
    }

    /// <summary>
    /// 扫描识别
    /// </summary>
    [Serializable]
    public class ScanLabelData
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 标签主单位数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 主单位Id
        /// </summary>
        public double ItemUnitId { get; set; }

        /// <summary>
        /// 主单位名称
        /// </summary>
        public string ItemUnitName { get; set; }
    }
}
