using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Report.WipResourceBoard.APIModels
{
    /// <summary>
    /// 工单任务情况
    /// </summary>
    [Serializable]
    public class WoOrderTaskInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Core.WorkOrders.WorkOrderState State { get; set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public string PlanBeginDate { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public string PlanEndDate { get; set; }
    }

    /// <summary>
    /// 产线在制工单
    /// </summary>
    [Serializable]
    public class WipWorkOrder
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// 产品机型
        /// </summary>
        public string ProModel { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public string PlanBeginDate { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public string PlanEndDate { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishQty { get; set; }
    }

    /// <summary>
    /// 在制工单一次通过率
    /// </summary>
    [Serializable]
    public class WipWorkOrderPass
    {
        /// <summary>
        /// 过板数
        /// </summary>
        public decimal InputQty { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal PassQty { get; set; }

        /// <summary>
        /// 不良数
        /// </summary>
        public decimal FailedQty { get; set; }
    }

    /// <summary>
    /// 缺陷统计
    /// </summary>
    [Serializable]
    public class DefectCount
    {
        /// <summary>
        /// 缺陷名称
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 缺陷统计数
        /// </summary>
        public decimal Count { get; set; }
    }

    /// <summary>
    /// 时段生产效率
    /// </summary>
    [Serializable]
    public class WipProductEfficiency
    {
        /// <summary>
        /// 目标产出
        /// </summary>
        public List<decimal> TargetProduct { get; set; } = new List<decimal>();

        /// <summary>
        /// 实际产出
        /// </summary>
        public List<decimal> ActualProduct { get; set; } = new List<decimal>();

        /// <summary>
        /// 生产效率
        /// </summary>
        public List<decimal> Efficiency { get; set; } = new List<decimal>();

        /// <summary>
        /// 班次时间段
        /// </summary>
        public List<string> TimeRange { get; set; } = new List<string>();
    }

    /// <summary>
    /// 缺陷TOP5柏拉图
    /// </summary>
    [Serializable]
    public class DefectPlato
    {
        /// <summary>
        /// 缺陷统计
        /// </summary>
        public List<DefectCount> DefectCountList { get; set; } = new List<DefectCount>();

        /// <summary>
        /// 柏拉图折线
        /// </summary>
        public List<decimal> Plato { get; set; } = new List<decimal>();
    }
}
