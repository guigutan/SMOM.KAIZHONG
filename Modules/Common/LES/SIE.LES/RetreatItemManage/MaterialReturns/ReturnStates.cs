using SIE.ObjectModel;

namespace SIE.LES.RetreatItemManage.MaterialReturns
{
    /// <summary>
    /// 退料状态
    /// </summary>
    public enum   ReturnStates
    {
        /// <summary>
        /// 待提交
        /// </summary>
        [Label("待提交")]
        TobeSubmitted=10,

        /// <summary>
        /// 已撤销
        /// </summary>
        [Label("已撤销")]
        Revoked =20,

        /// <summary>
        /// 已提交
        /// </summary>
        [Label("已提交")]
        Submitted =30

    }
}
