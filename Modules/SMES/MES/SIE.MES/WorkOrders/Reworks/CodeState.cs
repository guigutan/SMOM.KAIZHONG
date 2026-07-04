using SIE.ObjectModel;

namespace SIE.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 条码状态
    /// </summary>
    public enum CodeState
    {
        /// <summary>
        /// 已关联
        /// </summary>
        [Label("已关联")]
        Associated,

        /// <summary>
        /// 未关联
        /// </summary>
        [Label("未关联")]
        NotAssociated,
    }
}