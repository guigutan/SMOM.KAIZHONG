using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划类型
    /// </summary>
    public enum CheckPlanType
    {
        /// <summary>
        /// 日
        /// </summary>
        [Label("日")]
        Day = 0,
        /// <summary>
        /// 班
        /// </summary>
        [Label("班")]
        Shift = 1,
        /// <summary>
        /// 频次
        /// </summary>
        [Label("频次")]
        Time = 2,
    }
}
