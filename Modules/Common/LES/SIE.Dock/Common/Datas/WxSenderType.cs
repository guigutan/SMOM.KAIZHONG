using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.Common.Datas
{
    /// <summary>
    /// 通知类型-预约通知/排队通知
    /// </summary>
    public enum WxSenderType
    {
        /// <summary>
        /// 预约
        /// </summary>
        [Label("预约")]
        Appoint,

        /// <summary>
        /// 排队
        /// </summary>
        [Label("排队")]
        Queue,
    }
}
