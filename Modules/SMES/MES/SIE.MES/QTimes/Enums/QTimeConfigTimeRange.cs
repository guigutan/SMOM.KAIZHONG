using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Enums
{
    /// <summary>
    /// QT标准维护配置项时间枚举
    /// </summary>
    public enum QTimeConfigTimeRange
    {
        /// <summary>
        /// 当天
        /// </summary>
        [Label("当天")]
        Today = 0,

        /// <summary>
        /// 本周
        /// </summary>
        [Label("本周")]
        ThisWeek = 1,

        /// <summary>
        /// 本月
        /// </summary>
        [Label("本月")]
        ThisMonth = 2,

        /// <summary>
        /// 最近一月
        /// </summary>
        [Label("最近一月")]
        RecentlyMonth = 3,

        /// <summary>
        /// 本年
        /// </summary>
        [Label("本年")]
        ThisYear = 4,

        /// <summary>
        /// 自定义
        /// </summary>
        [Label("自定义")]
        Customize = 5,
    }
}
