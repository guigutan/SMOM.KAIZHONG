using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 治具状态
    /// </summary>
    public enum FixtureStatus
    {
        /// <summary>
        /// 在库
        /// </summary>
        [Label("在库")]
        InStorage = 10,
        /// <summary>
        /// 在线
        /// </summary>
        [Label("在线")]
        OnLine = 20,
    }
}
