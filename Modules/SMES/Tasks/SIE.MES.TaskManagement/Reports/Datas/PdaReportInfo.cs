using SIE.MES.TaskManagement.Reports.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    /// <summary>
    /// 分页物料数据模型
    /// </summary>
    [Serializable]
    public class PagingReportInfo
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 页数据数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 可报工列表
        /// </summary>
        public List<PdaReportInfo> ReportInfos { get; } = new List<PdaReportInfo>();
    }

    /// <summary>
    /// 报工信息
    /// </summary>
    [Serializable]
    public class PdaReportInfo
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 派工任务ID
        /// </summary>
        public double DispatchTaskId { get; set; }

        /// <summary>
        /// 派工任务号
        /// </summary>
        public string DispatchTaskNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }


        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }


        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginTime { get; set; }

    }


    /// <summary>
    /// 提交报工信息
    /// </summary>
    [Serializable]
    public class SubmitPdaReportInfo
    {
        /// <summary>
        /// 派工任务ID
        /// </summary>
        public double DispatchTaskId { get; set; }

      
        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty { get; set; }

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal GoodQty { get; set; }
        /// <summary>
        /// 废品数量
        /// </summary>
        public decimal ScrapQty { get; set; }
        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal ReworkQty { get; set; }

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty { get; set; }

        /// <summary>
        /// 批次标签
        /// </summary>
        public List<ReportSnInfo> SnInfos { get; set; } = new List<ReportSnInfo>();


        /// <summary>
        /// 报工员Id
        /// </summary>
        public double ReportEmployeeId { get; set; }
        /// <summary>
        /// 是否跳过前工序报工数量验证
        /// </summary>
        public bool IsSkipValidatePreQty { get; set; }


        /// <summary>
        /// 是否校验开机前置
        /// </summary>
        public bool IsValidatePrepare { get; set; }

        /// <summary>
        /// 是否完工(只有手动、扫码弹窗选择的时候否的时候才输入false)
        /// </summary>
        public bool IsTaskFinish { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId { get; set; }
        /// <summary>
        /// 是否自动上料(适用于成品工单包装报工)
        /// </summary>
        public bool IsAutoFeeding { get; set; }

        /// <summary>
        /// 是否手动报工
        /// </summary>
        public bool IsReportManual { get; set; } = true;

        /// <summary>
        /// 是否共模报工
        /// </summary>
        public bool IsCommonMode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType? SourceType { get; set; }

        /// <summary>
        /// 队列Id
        /// </summary>
        public double QueueId { get; set; }
    }

    /// <summary>
    /// 报工标签信息
    /// </summary>
    [Serializable]
    public class ReportSnInfo
    {

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal GoodQty { get; set; }
        /// <summary>
        /// 废品数量
        /// </summary>
        public decimal ScrapQty { get; set; }
        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal ReworkQty { get; set; }

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty { get; set; }

        /// <summary>
        /// 批次标签
        /// </summary>
        public string Sn { get; set; }
    }
}
