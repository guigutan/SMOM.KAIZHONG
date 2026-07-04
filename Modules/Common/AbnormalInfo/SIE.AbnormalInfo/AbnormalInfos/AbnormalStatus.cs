using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常状态
    /// </summary>
    [Serializable]
    public enum AbnormalStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [Label("待处理")]
        ToProcess,
        /// <summary>
        /// 处理中
        /// </summary>
        [Label("处理中")]
        Processing,
        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close,
    }
}
