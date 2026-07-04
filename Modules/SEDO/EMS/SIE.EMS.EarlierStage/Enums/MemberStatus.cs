using SIE.ObjectModel;

namespace SIE.EMS.EarlierStage.Enums
{
    /// <summary>
    /// 项目成员状态
    /// </summary>
    public enum MemberStatus
    {
        /// <summary>
        /// 在岗
        /// </summary>
        [Label("在岗")]
        In = 10,
        /// <summary>
        /// 离岗
        /// </summary>
        [Label("离岗")]
        Out = 20,
    }
}