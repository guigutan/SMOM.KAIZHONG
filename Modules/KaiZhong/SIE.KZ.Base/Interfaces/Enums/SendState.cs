using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Enums
{
    /// <summary>
    /// 推送状态
    /// </summary>
    public enum SendState
    {
        /// <summary>
        /// 未推送
        /// </summary>
        [Label("未推送")]
        NoSend = 0,

        /// <summary>
        /// 推送失败
        /// </summary>
        [Label("推送失败")]
        SendFail = 1,
        /// <summary>
        /// 推送成功
        /// </summary>
        [Label("推送成功")]
        SendSuccess = 2,
        /// <summary>
        /// 忽略处理
        /// </summary>
        [Label("忽略处理")]
        SendIgnore = 3,
    }
}
