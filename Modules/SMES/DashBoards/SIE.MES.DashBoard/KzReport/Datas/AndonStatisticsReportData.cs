using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 安灯统计报表请求参数
    /// </summary>
    [Serializable]
    public class RequestAndonStatisticsReportData
    {
        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 产部
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// 是否展开安灯类型
        /// </summary>
        public string ExpandAndonType { get; set; }

        /// <summary>
        /// 日期范围
        /// </summary>
        public DateRange DateRange { get; set; }
    }

    /// <summary>
    /// 安灯统计报表返回数据
    /// </summary>
    [Serializable]
    public class AndonStatisticsReportData
    {
        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 产部
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public string AndonType { get; set; }

        /// <summary>
        /// 异常停机时长(H)
        /// </summary>
        public decimal AbnormalDowntime { get; set; }

        /// <summary>
        /// 安灯次数
        /// </summary>
        public int AndonCount { get; set; }

        /// <summary>
        /// 平均响应时长(H)
        /// </summary>
        public decimal AvgResponseTime { get; set; }

        /// <summary>
        /// 平均处理时长(H)
        /// </summary>
        public decimal AvgHandleTime { get; set; }

        /// <summary>
        /// 平均验收时长(H)
        /// </summary>
        public decimal AvgCheckTime { get; set; }

        /// <summary>
        /// 超时分钟数
        /// </summary>
        public decimal? OverTime { get; set; }

        /// <summary>
        /// 超30min未关闭个数 (30≤时长＜60min)
        /// </summary>
        public int Over30Min { get; set; }

        /// <summary>
        /// 超60min未关闭个数 (60≤时长＜120min)
        /// </summary>
        public int Over60Min { get; set; }

        /// <summary>
        /// 超120min未关闭个数 (120≤时长＜240min)
        /// </summary>
        public int Over120Min { get; set; }

        /// <summary>
        /// 超240min未关闭个数 (240≤时长)
        /// </summary>
        public int Over240Min { get; set; }
    }
}
