using SIE.ObjectModel;

namespace SIE.Kit.MES.SingleLabels
{
    /// <summary>
    /// 条码来源类型
    /// </summary>
    public enum SingleLabelSourceType
    {
        /// <summary>
        /// PO
        /// </summary>
        [Label("PO")]
        PO,

        /// <summary>
        /// 工单
        /// </summary>
        [Label("WO")]
        WO,
    }
}