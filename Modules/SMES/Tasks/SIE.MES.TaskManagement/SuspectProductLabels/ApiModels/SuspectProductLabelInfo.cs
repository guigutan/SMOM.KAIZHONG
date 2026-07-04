using System;

namespace SIE.MES.TaskManagement.SuspectProductLabels.ApiModels
{
    /// <summary>
    /// 可疑品标签信息
    /// </summary>
    [Serializable]
    public class SuspectProductLabelInfo
    {
        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        public double SuspectProductLabelId { get; set; }
        /// <summary>
        /// 可疑品标签
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty { get; set; }
        /// <summary>
        /// 是否需要MRB报告
        /// </summary>
        public bool NeedMrbReport { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ItemShortDescription { get; set; }

        /// <summary>
        /// 父级旧料号
        /// </summary>
        public string Bismt { get; set; }
    }
}
