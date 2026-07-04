using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 借调单状态
    /// </summary>
    public enum OnLoanState
    {
        /// <summary>
        /// 待批复
        /// </summary>
        [Label("待批复")]
        ToBeApproved = 0,

        /// <summary>
        /// 已同意
        /// </summary>
        [Label("已同意")]
        Agree = 1,

        /// <summary>
        /// 已拒绝
        /// </summary>
        [Label("已拒绝")]
        Refuse = 2,

        /*/// <summary>
        /// 已过期--顾问已取消该状态
        /// </summary>
        [Label("已过期")]
        LoseEfficacy = 3,*/

        /// <summary>
        /// 已撤销
        /// </summary>
        [Label("已撤销")]
        Cancel = 4
    }
}
