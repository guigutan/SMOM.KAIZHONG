using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 单位类型
    /// </summary>
    [Serializable]
    public enum UnitType
    {
        /// <summary>
        /// 分钟
        /// </summary>
        [Label("分钟")]
        Minute = 0,

        /// <summary>
        /// 小时
        /// </summary>
        [Label("小时")]
        Hours = 1,

        /// <summary>
        /// 天
        /// </summary>
        [Label("天")]
        Days = 2,
    }
}
