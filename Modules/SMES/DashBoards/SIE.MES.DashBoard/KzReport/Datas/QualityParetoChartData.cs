using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 质量帕累托图-请求参数
    /// </summary>
    [Serializable]
    public class RequestQualityParetoChartData
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
        /// 日期范围
        /// </summary>
        public DateRange DateRange { get; set; }
    }

    /// <summary>
    /// 质量帕累托图-响应数据
    /// </summary>
    [Serializable]
    public class ResponseQualityParetoChartData
    {
        /// <summary>
        /// 不良品帕累托图表格数据
        /// </summary>
        public List<DefectProductParetoData> DefectProductTableList { get; set; }

        /// <summary>
        /// 缺陷名称帕累托图表格数据
        /// </summary>
        public List<DefectNameParetoData> DefectNameTableList { get; set; }
    }

    /// <summary>
    /// 不良品帕累托图数据（按产品分组）
    /// </summary>
    [Serializable]
    public class DefectProductParetoData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

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
        /// 缺陷代码
        /// </summary>
        public string DefectCode { get; set; }

        /// <summary>
        /// 缺陷名称
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 占比总和
        /// </summary>
        public decimal RateSum { get; set; }
    }

    /// <summary>
    /// 缺陷名称帕累托图数据（按缺陷分组）
    /// </summary>
    [Serializable]
    public class DefectNameParetoData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string DefectCode { get; set; }

        /// <summary>
        /// 缺陷名称
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 占比总和
        /// </summary>
        public decimal RateSum { get; set; }
    }

    /// <summary>
    /// 质量帕累托图-工厂返回原始数据
    /// </summary>
    [Serializable]
    public class QualityParetoChartFactoryData
    {
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
        /// 缺陷代码
        /// </summary>
        public string DefectCode { get; set; }

        /// <summary>
        /// 缺陷名称
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 判定结果（Good/Scrap/Repair）
        /// </summary>
        public string JudgmentResult { get; set; }
    }
}
