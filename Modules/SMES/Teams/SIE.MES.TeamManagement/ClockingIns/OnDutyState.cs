using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 出勤状态
    /// </summary>
    public enum OnDutyState
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal,

        /// <summary>
        /// 缺勤
        /// </summary>
        [Label("异常")]
        Absence,

        /// <summary>
        /// 休息
        /// </summary>
        [Label("休息")]
        Rest,
    }
}