using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    ///报工状态
    /// </summary>
    public enum ReportState
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Label("未开始")]
        NotStarted = 0,

        /// <summary>
        /// 部分报工
        /// </summary>
        [Label("部分报工")]
        PartReport = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Finish = 2
    }
}
