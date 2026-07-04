using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 点检(保养)执行结果
    /// </summary>
    public enum ExeResult
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Label("合格")]
        Successed = 10,

        /// <summary>
        /// 不合格
        /// </summary>
        [Label("不合格")]
        Failed = 20,

    }
}
