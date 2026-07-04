using SIE.ObjectModel;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.IqcPick.Model
{
    /// <summary>
    /// 挑选来料参数
    /// </summary>
    public class DoPickEvent
    {
        /// <summary>
        /// 接收记录Id
        /// </summary> 
        public double? InspectionId { get; set; }

        /// <summary>
        /// 单据挑选状态
        /// </summary>
        public PickStatus PickStatus { get; set; }

        /// <summary>
        /// 挑选单号
        /// </summary>
        [Label("挑选单号")]
        public string IqcPickBillNo { get; set; }

        /// <summary>
        /// 原检验单号
        /// </summary>
        [Label("原检验单号")]
        public string SourceIqcBillNo { get; set; }

        /// <summary>
        /// Asn单号
        /// </summary>
        [Label("Asn单号")]
        public string AsnNo { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public string BatchNo { get; set; }

        /// <summary>
        /// 挑选数量
        /// </summary>
        [Label("挑选数量")]
        public decimal PickQty { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        public decimal PassQty { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public decimal FailQty { get; set; }

        /// <summary>
        /// 不良SN号
        /// </summary>
        public List<string> FailSNList { get; set; }
    }
}
