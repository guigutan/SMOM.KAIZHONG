using System;
using System.Collections.Generic;

namespace SIE.EMS.Report.WorkOrderExcuteReports
{
    /// <summary>
    /// 工单执行统计报表信息
    /// </summary>
    [Serializable]
    public class WorkOrderExcuteReportInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderExcuteReportInfo()
        {
            ChartInfo = new List<ChartInfo>();
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string err { get; set; }

        /// <summary>
        /// 汇总数据
        /// </summary>
        public CounInfo CounInfo { get; set; }
        /// <summary>
        /// 折线图数据
        /// </summary>
        public List<ChartInfo> ChartInfo { get; set; }
        /// <summary>
        /// 表格数据
        /// </summary>
        public TableInfo TableInfo { get; set; }
    }


    /// <summary>
    /// 汇总数据
    /// </summary>
    [Serializable]
    public class CounInfo
    {
        /// <summary>
        /// 工单数
        /// </summary>
        public int WorkCount { get; set; }

        /// <summary>
        /// 已完成工单数
        /// </summary>
        public int CompleteCount { get; set; }

        /// <summary>
        /// 待完成工单数
        /// </summary>
        public int WaitCompleteCount { get; set; }

        /// <summary>
        /// 完成率
        /// </summary>
        public decimal CompleteRate { get; set; }

    }


    /// <summary>
    /// 折线图数据
    /// </summary>
    [Serializable]
    public class ChartInfo
    {
        /// <summary>
        /// 统计数据
        /// </summary>
        public DateTime SummaryTime { get; set; }
        /// <summary>
        /// X轴绑定的数据
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// 工单总数
        /// </summary>
        /// 
        public int WorkOrderQty { get; set; }
        /// <summary>
        /// 工单完成数
        /// </summary>
        public int CompleteQty { get; set; }

        /// <summary>
        /// 工单完成率
        /// </summary>
        public decimal CompleteRate{ get; set; }

    }

    /// <summary>
    /// 表格数据
    /// </summary>

    [Serializable]
    public class TableInfo
    {
        /// <summary>
        /// 动态列名称
        /// </summary>
        public List<string> ClounNameList { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public List<List<string>> Datas { get; set; }
    }

}
