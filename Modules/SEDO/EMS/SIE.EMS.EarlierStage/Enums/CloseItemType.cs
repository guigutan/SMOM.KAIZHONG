using SIE.ObjectModel;

namespace SIE.EMS.EarlierStage.Enums
{
    /// <summary>
    /// 结项类型
    /// </summary>
    public enum CloseItemType
    {
        /// <summary>
        /// 项目完结
        /// </summary>
        [Label("项目完结")]
        ProjectClosing = 0,
        /// <summary>
        /// 项目中止
        /// </summary>
        [Label("项目中止")]
        ProjectBreakOff = 1
    }
}
