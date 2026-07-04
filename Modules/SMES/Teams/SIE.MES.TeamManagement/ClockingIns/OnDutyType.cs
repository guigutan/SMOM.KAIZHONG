using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 出勤状态
    /// </summary>
    public enum OnDutyType
    {
        /// <summary>
        /// 取最早打卡记录
        /// </summary>
        [Label("取最早打卡记录")]
        Earliest,

        /// <summary>
        /// 取最晚打卡记录
        /// </summary>
        [Label("取最晚打卡记录")]
        Latest,
    }
}
