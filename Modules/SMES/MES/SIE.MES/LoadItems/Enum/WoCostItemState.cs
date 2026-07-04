using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.LoadItems.Enum
{
    /// <summary>
    /// 工单耗用量状态枚举
    /// </summary>
    public enum WoCostItemState
    {
        /// <summary>
        /// 未扣料
        /// </summary>
        [Label("未扣料")]
        ToSubmit = 10,

        /// <summary>
        /// 扣料成功
        /// </summary>
        [Label("扣料成功")]
        Submitted = 20,

        /// <summary>
        /// 扣料失败
        /// </summary>
        [Label("扣料失败")]
        FailSubmit = 30,

        /// <summary>
        /// 强制关闭
        /// </summary>
        [Label("强制关闭")]
        Close = 40,
    }
}
