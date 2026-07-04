using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工顺序
    /// </summary>
    [Flags]
    public enum ReportOrder
    {
        [Label("按计划开始时间正序执行")]
        BeginDate = 1,

        [Label("按紧急程度优先执行")]
        Priority = 2
    }
}