using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys.Enums
{
    /// <summary>
    /// 退料状态
    /// </summary>
    public enum ReStatus
    {
        /// <summary>
        /// 保存
        /// </summary>
        [Label("保存")]
        Saved = 0,

        /// <summary>
        /// 已提交
        /// </summary>
        [Label("已提交")]
        Submitted = 1,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Canceled = 2,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Finished = 3,
    }
}
