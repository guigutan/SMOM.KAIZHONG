using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Enum
{
    /// <summary>
    /// 消息推送节点枚举
    /// </summary>
    public enum AndonTypeMessageSendState
    {
        /// <summary>
        /// 待响应
        /// </summary>
        [Label("待响应")]
        Standby = 10,

        /// <summary>
        /// 处理中
        /// </summary>
        [Label("处理中")]
        Processing = 20,

        /// <summary>
        /// 待验收
        /// </summary>
        [Label("待验收")]
        ToAccepted = 30,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Closed = 40,

        /// <summary>
        /// 驳回
        /// </summary>
        [Label("驳回")]
        Reject = 50,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel = 60,
    }
}
