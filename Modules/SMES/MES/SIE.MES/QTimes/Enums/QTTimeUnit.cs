using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Enums
{
    /// <summary>
    /// QT时间枚举
    /// </summary>
    public enum QTTimeUnit
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
        Hour = 1,

        /// <summary>
        /// 天
        /// </summary>
        [Label("天")]
        Day = 2,
    }
}
