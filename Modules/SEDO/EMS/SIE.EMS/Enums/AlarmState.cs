using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 报警状态
    /// </summary>
    public enum AlarmState
    {
        /// <summary>
        /// 开启
        /// </summary>
        [Label("开启")]
        Alarm = 1,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close = 2,

        /// <summary>
        /// 无效
        /// </summary>
        [Label("无效")]
        Invalid = 3,


    }
}
