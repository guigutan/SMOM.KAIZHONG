using System;
using System.Collections.Generic;

namespace SIE.MES.TaskManagement.SuspectProductLabels.ApiModels
{
    /// <summary>
    /// 可疑品标签数据
    /// </summary>
    [Serializable]
    public class SuspectProductLabelData
    {
        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        public double SuspectProductLabelId { get; set; }
        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal GoodQty { get; set; }
        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }
        /// <summary>
        /// 报废明细列表
        /// </summary>
        public List<SuspectProductLabelDetailData> ScrapDetailList { get; set; }
        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal RepairQty { get; set; }
        /// <summary>
        /// 返工明细列表
        /// </summary>
        public List<SuspectProductLabelDetailData> RepairDetailList { get; set; }
        /// <summary>
        /// 附件Id列表
        /// </summary>
        public List<double> AttachmentIdList { get; set; }

        /// <summary>
        /// 是否完工(只有手动、扫码弹窗选择的时候否的时候才输入false)
        /// </summary>
        public bool IsTaskFinish { get; set; }
    }

    /// <summary>
    /// 可疑品标签明细数据
    /// </summary>
    [Serializable]
    public class SuspectProductLabelDetailData
    {
        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double? DefectId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
