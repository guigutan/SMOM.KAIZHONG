using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.AndonStatisticsReports
{
    /// <summary>
    /// 汇总维度
    /// </summary>
    public enum SummaryDimension
    {
        /// <summary>
        /// 安灯大类
        /// </summary>
        [Label("安灯大类")]
        AndonClass,

        /// <summary>
        /// 安灯类型
        /// </summary>
        [Label("安灯类型")]
        AndonType,

        /// <summary>
        /// 安灯编码
        /// </summary>
        [Label("安灯编码")]
        AndonCode,



    }
}
