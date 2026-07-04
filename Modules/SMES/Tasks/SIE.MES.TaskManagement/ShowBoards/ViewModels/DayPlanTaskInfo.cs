using System;

namespace SIE.MES.TaskManagement.ShowBoards.ViewModels
{
    /// <summary>
    /// 日计划任务信息
    /// </summary>
    [Serializable]
    public class DayPlanTaskInfo
    {
        /// <summary>
        /// 班组/员工组 GroupName
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 工单类型 Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 任务数 DispatchQty
        /// </summary>
        public decimal DispatchQty { get; set; }

        /// <summary>
        /// 派工总数 DispatchTotalQty
        /// </summary>
        public decimal DispatchTotalQty { get; set; }

        /// <summary>
        /// 计划日期 PlanDate
        /// </summary>
        public string PlanDate { get; set; }

        /// <summary>
        /// 完工任务数 FinishedDispatchQty
        /// </summary>
        public decimal FinishedDispatchQty { get; set; }

        /// <summary>
        /// 完工数 FinishedTotalQty
        /// </summary>
        public decimal FinishedTotalQty { get; set; }

        /// <summary>
        /// 完成率 FinishedRate
        /// </summary>
        public string FinishedRate { get; set; }
    }

    /// <summary>
    /// 计划任务统计信息(统计日月累计总数和完工数)
    /// </summary>
    [Serializable]
    public class PlanTaskTotalInfo
    {
        /// <summary>
        /// 日派工总数 DispatchTotalQtyOfDay
        /// </summary>
        public decimal DispatchTotalQtyOfDay { get; set; }

        /// <summary>
        /// 日完工数 FinishedTotalQtyOfDay
        /// </summary>
        public decimal FinishedTotalQtyOfDay { get; set; }

        /// <summary>
        /// 月派工总数 DispatchTotalQtyOfMonth
        /// </summary>
        public decimal DispatchTotalQtyOfMonth { get; set; }

        /// <summary>
        /// 月完工数 FinishedTotalQtyOfMonth
        /// </summary>
        public decimal FinishedTotalQtyOfMonth { get; set; }
    }

    /// <summary>
    /// 异常任务信息
    /// </summary>
    [Serializable]
    public class AbnormalTaskInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 工单类型 Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 任务数 DispatchQty
        /// </summary>
        public decimal DispatchQty { get; set; }

        /// <summary>
        /// 计划日期 PlanDate
        /// </summary>
        public string PlanDate { get; set; }

        /// <summary>
        /// 未完工任务
        /// </summary>
        public decimal UnFinishedQty { get; set; }

        /// <summary>
        /// 超期天数
        /// </summary>
        public double ExtendedDays { get; set; }
    }

    /// <summary>
    /// 产能工时信息
    /// </summary>
    [Serializable]
    public class CapacityHourInfo
    {
        /// <summary>
        /// 日期 PlanDate
        /// </summary>
        public string PlanDate { get; set; }

        /// <summary>
        /// 任务工时
        /// </summary>
        public decimal Hour { get; set; }

        /// <summary>
        /// 开始工时
        /// </summary>
        public decimal sHour { get; set; }

        /// <summary>
        /// 结束工时
        /// </summary>
        public decimal eHour { get; set; }

        /// <summary>
        /// 报工总数 FinishedTotalQty
        /// </summary>
        public decimal FinishedTotalQty { get; set; }

        /// <summary>
        /// 开始报工总数
        /// </summary>
        public decimal sFinishedTotalQty { get; set; }

        /// <summary>
        /// 结束报工总数
        /// </summary>

        public decimal eFinishedTotalQty { get; set; }
    }

    /// <summary>
    /// 日生产任务信息
    /// </summary>
    [Serializable]
    public class DayProduceTaskInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 工单类型 Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 任务数 DispatchQty
        /// </summary>
        public decimal DispatchQty { get; set; }

        /// <summary>
        /// 派工总数 DispatchTotalQty
        /// </summary>
        public decimal DispatchTotalQty { get; set; }

        /// <summary>
        /// 完工任务数 FinishedDispatchQty
        /// </summary>
        public decimal FinishedDispatchQty { get; set; }

        /// <summary>
        /// 完工数 FinishedTotalQty
        /// </summary>
        public decimal FinishedTotalQty { get; set; }

        /// <summary>
        /// 完成率 FinishedRate
        /// </summary>
        public string FinishedRate { get; set; }

        /// <summary>
        /// 超期天数
        /// </summary>
        public double ExtendedDays { get; set; }
    }

    /// <summary>
    /// 生产缺陷信息
    /// </summary>
    [Serializable]
    public class DayDefectInfo
    {
        /// <summary>
        /// 缺陷名称
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 缺陷数量
        /// </summary>
        public decimal DefectQty { get; set; }

        /// <summary>
        /// 缺陷占比数
        /// </summary>
        public decimal DefectSumQty { get; set; }

        /// <summary>
        /// 缺陷占比率
        /// </summary>
        public decimal DefectRate { get; set; }
    }

    /// <summary>
    /// 缺陷良率信息
    /// </summary>
    [Serializable]
    public class DefectRateInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string PlanDate { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal OkQty { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 良率
        /// </summary>
        public decimal OkRate { get; set; }
    }
}
