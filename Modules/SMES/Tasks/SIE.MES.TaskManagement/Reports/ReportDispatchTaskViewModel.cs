using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工任务报表数据类
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ReportDispatchTaskViewModelCriteria))]
    [Label("任务统计报表")]
    public class ReportDispatchTaskViewModel : ViewModel
    {
        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 任务数量
        /// </summary>
        public double DispatchQty { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public double OrderQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public double FinishQty { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public double OkQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public double NgQty { get; set; }

        /// <summary>
        ///  工时
        /// </summary>
        public double Hour { get; set; }

        /// <summary>
        /// 超时任务数
        /// </summary>
        public double DelayQty { get; set; }

        /// <summary>
        /// 未关闭任务数
        /// </summary>
        public double NoCloseQty { get; set; }

        /// <summary>
        /// 任务完工率
        /// </summary>
        public double DispatchFinishPer { get; set; }

        /// <summary>
        /// 产品完工率
        /// </summary>
        public double ProductFinishPer { get; set; }

        /// <summary>
        /// 准时达成率
        /// </summary>
        public double OnTimePer { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        public double OkPer { get; set; }
    }

    /// <summary>
    /// 任务统计报表信息
    /// </summary>
    public class DispatchReportInfo
    {
        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 任务数量
        /// </summary>
        public double DispatchQty { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public double OrderQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public double FinishQty { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public double OkQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public double NgQty { get; set; }

        /// <summary>
        ///  工时
        /// </summary>
        public double Hour { get; set; }

        /// <summary>
        /// 超时任务数
        /// </summary>
        public double DelayQty { get; set; }

        /// <summary>
        /// 未关闭任务数
        /// </summary>
        public double NoCloseQty { get; set; }

        /// <summary>
        /// 任务完工率
        /// </summary>
        public double DispatchFinishPer { get; set; }

        /// <summary>
        /// 产品完工率
        /// </summary>
        public double ProductFinishPer { get; set; }

        /// <summary>
        /// 准时达成率
        /// </summary>
        public double OnTimePer { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        public double OkPer { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 列标题
        /// </summary>
        public string HeadTitle { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public double Qty { get; set; }
    }
}
