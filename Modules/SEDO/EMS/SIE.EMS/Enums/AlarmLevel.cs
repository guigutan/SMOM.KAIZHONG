using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 报警级别
    /// </summary>
    public enum AlarmLevel
    {
        /// <summary>
        /// 提示
        /// </summary>
        [Label("提示")]
        Info = 1,

        /// <summary>
        /// 轻微
        /// </summary>
        [Label("轻微")]
        Minor = 2,

        /// <summary>
        /// 一般
        /// </summary>
        [Label("一般")]
        Medium = 3,

        /// <summary>
        /// 严重
        /// </summary>
        [Label("严重")]
        Major = 4,

        /// <summary>
        /// 紧急
        /// </summary>
        [Label("紧急")]
        Serious = 5
    }
}
