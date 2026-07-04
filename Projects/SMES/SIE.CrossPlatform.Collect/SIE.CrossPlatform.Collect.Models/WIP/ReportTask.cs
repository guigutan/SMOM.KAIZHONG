using System;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 任务列表明细
    /// </summary>
    [Serializable]
    public class ReportTask
    {
        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get;
            set;
        }


        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get;
            set;
        }


        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get;
            set;
        }

        /// <summary>
        /// 待报工
        /// </summary>
        public decimal ToReportQty
        {
            get;
            set;
        }

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty
        {
            get;
            set;
        }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get;
            set;
        }


        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get;
            set;
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority
        {
            get;
            set;
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndTime
        {
            get;
            set;
        }
    }
}
