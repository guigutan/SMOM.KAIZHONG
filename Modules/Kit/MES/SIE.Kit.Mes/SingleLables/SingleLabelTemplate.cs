using System;

namespace SIE.Kit.MES.SingleLabels
{
    /// <summary>
    /// 单体条码模板简单实体对象
    /// </summary>
    [Serializable]
    public class SingleLabelTemplate
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchCode { get; set; }

        /// <summary>
        /// 打印日期
        /// </summary>
        public DateTime? PrintDate { get; set; }

        /// <summary>
        /// 来源Id
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// 来源号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 条码来源类型
        /// </summary>
        public SingleLabelSourceType SourceType { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 标签状态
        /// </summary>
        public LabelState LabelState { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }
    }
}
