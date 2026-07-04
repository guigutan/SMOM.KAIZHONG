using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Enum
{
    /// <summary>
    /// 安灯管理操作记录
    /// </summary>
    public enum AndonManageOperateType
    {
        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel = 0,

        /// <summary>
        /// 响应
        /// </summary>
        [Label("响应")]
        Response = 1,

        /// <summary>
        /// 转派
        /// </summary>
        [Label("转派")]
        Reassignment = 2,

        /// <summary>
        /// 处理完成
        /// </summary>
        [Label("处理完成")]
        Handle = 3,

        /// <summary>
        /// 验收
        /// </summary>
        [Label("验收")]
        Check = 4,

        /// <summary>
        /// 驳回
        /// </summary>
        [Label("驳回")]
        Reject = 5,

        /// <summary>
        /// 触发
        /// </summary>
        [Label("触发")]
        Add = 6,

        /// <summary>
        /// 安灯名称变更
        /// </summary>
        [Label("安灯名称变更")]
        AndonNameChange = 7,
    }
}
