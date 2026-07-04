using System;
using System.Collections.Generic;

namespace SIE.Web.MES.DashBoard.Reports.LineFPY
{
    /// <summary>
    /// 产线直通率信息
    /// </summary>
    public class ProductionLineRateInfo
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 直通率
        /// </summary>
        public decimal DirectRate { get; set; }

        /// <summary>
        /// 折线图信息列表
        /// </summary>
        public List<LineChartInfo> LineChartInfoList { get; } = new List<LineChartInfo>();

        /// <summary>
        /// 折线图目标\警告设置信息列表
        /// </summary>
        public LineChartSettingInfo LineChartSettingInfo { get; set; }
    }

    /// <summary>
    /// 折线图信息
    /// </summary>
    public class LineChartInfo
    {
        /// <summary>
        /// 采集日期
        /// </summary>
        public string XDate { get; set; }

        /// <summary>
        /// 直通率
        /// </summary>
        public decimal YData { get; set; }

        /// <summary>
        /// 目标值
        /// </summary>
        public decimal YDesired { get; set; }

        /// <summary>
        /// 预警值
        /// </summary>
        public decimal YAlarm { get; set; }
    }

    /// <summary>
    /// 折线图目标\警告设置信息
    /// </summary>
    public class LineChartSettingInfo
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 直通率期望值
        /// </summary>
        public decimal Desired { get; set; }

        /// <summary>
        /// 直通率预警值
        /// </summary>
        public decimal Alarm { get; set; }
    }

    /// <summary>
    /// 工序直通率信息
    /// </summary>
    public class ProcessDirectRateInfo
    {
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 良品通过率
        /// </summary>
        public decimal PasssRate { get; set; }
    }

    /// <summary>
    /// 缺陷分类信息
    /// </summary>
    public class DefectCategoryInfo
    {
        /// <summary>
        /// 分类编号
        /// </summary>
        public int CategoryNo { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类颜色
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// 分类宽度
        /// </summary>
        public double ColumnWidth { get; set; }

        /// <summary>
        /// 缺陷编码信息列表
        /// </summary>
        public List<DefectCodeInfo> DefectCodeList { get; } = new List<DefectCodeInfo>();
    }

    /// <summary>
    /// 缺陷编码信息
    /// </summary>
    public class DefectCodeInfo
    {
        /// <summary>
        /// 缺陷编码名称
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 缺陷编码占据高度
        /// </summary>
        public double RowHeight { get; set; }

        /// <summary>
        /// 文本高度
        /// </summary>
        public double LineHeight { get; set; }

        /// <summary>
        /// 是否是缺陷分类中最后一个缺陷编码
        /// </summary>
        public bool IsLast { get; set; }
    }

    /// <summary>
    /// 缺陷信息
    /// </summary>
    public class DefectInfo
    {
        /// <summary>
        /// 缺陷编码名称
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 缺陷数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 缺陷累计数
        /// </summary>
        public decimal CumQty { get; set; }

        /// <summary>
        /// 缺陷累计比例
        /// </summary>
        public decimal CumPercent { get; set; }
    }

    /// <summary>
    /// 工序良品\不良品统计信息
    /// </summary>
    public class ProcessStatisticsInfo
    {
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal PassQty { get; set; }

        /// <summary>
        /// 不良品数量
        /// </summary>
        public decimal FailedQty { get; set; }
    }

    /// <summary>
    /// 工序相关信息
    /// </summary>
    public class ProcessRelatedInfo
    {
        /// <summary>
        /// 工序直通率信息列表
        /// </summary>
        public List<ProcessDirectRateInfo> ProcessDirectRateInfoList { get; } = new List<ProcessDirectRateInfo>();

        /// <summary>
        /// 缺陷信息列表
        /// </summary>
        public List<DefectInfo> DefectInfoList { get; } = new List<DefectInfo>();

        /// <summary>
        /// 工序良品\不良品统计信息列表
        /// </summary>
        public List<ProcessStatisticsInfo> ProcessStatisticsInfoList { get; } = new List<ProcessStatisticsInfo>();
    }
}
