using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 质量不良统计报表-请求参数
    /// </summary>
    [Serializable]
    public class RequestQualityDefectReportData
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
        /// 工序
        /// </summary>
        public List<string> Process { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string OldPartNumber { get; set; }

        /// <summary>
        /// 是否返工单
        /// </summary>
        public string IsReWork { get; set; }

        /// <summary>
        /// 日期范围
        /// </summary>
        public DateRange DateRange { get; set; }
    }

    /// <summary>
    /// 质量不良统计报表-响应数据
    /// </summary>
    [Serializable]
    public class QualityDefectReportData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 产部
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string OldPartNumber { get; set; }

        /// <summary>
        /// 总产量（万）
        /// </summary>
        public decimal TotalOutput { get; set; }

        /// <summary>
        /// 报废总量（万）
        /// </summary>
        public decimal TotalScrap { get; set; }

        /// <summary>
        /// 返工总量（万）
        /// </summary>
        public decimal TotalRework { get; set; }

        /// <summary>
        /// 可疑品数量（万）
        /// </summary>
        public decimal TotalSuspect { get; set; }

        /// <summary>
        /// 报废率（%）
        /// </summary>
        public decimal ScrapRate { get; set; }

        /// <summary>
        /// 可疑品率（%）
        /// </summary>
        public decimal SuspectRate { get; set; }

        /// <summary>
        /// 一次下线合格率（%）
        /// </summary>
        public decimal FirstPassYield { get; set; }
    }

    /// <summary>
    /// 质量不良统计报表-工厂返回数据
    /// </summary>
    [Serializable]
    public class QualityDefectReportFactoryData
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
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string OldPartNumber { get; set; }

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal RecordNgQty { get; set; }

        /// <summary>
        /// 返工数
        /// </summary>
        public decimal ReworkQty { get; set; }

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty { get; set; }
    }
}
