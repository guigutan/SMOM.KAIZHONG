using SIE.ObjectModel;
using SIE.Wpf.Common.Diagram;
using System;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.WoStatus
{
    /// <summary>
    /// 工单状态实体
    /// </summary>
    public class WoStatusEntity : ObservableObject
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double ID
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public string No
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 计划
        /// </summary>
        public int PlanIndex
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 实际
        /// </summary>
        public int RealIndex
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 完成比计划
        /// </summary>
        public string ComAndPlan
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 合格率
        /// </summary>
        public string PassRate
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 完成率
        /// </summary>
        public string CompleteRate
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 完成进度条数值
        /// </summary>
        public string ProcessValue
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        public string ProductStatus
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? ActulEndDate
        {
            get { return GetProperty<DateTime?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActulStartDate
        {
            get { return GetProperty<DateTime?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public decimal FinishQty
        {
            get { return GetProperty<decimal>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 目标产能
        /// </summary>
        public string TargetQty
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 进度状态
        /// </summary>
        public string ProcessStatus
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 进度条颜色
        /// </summary>
        public string ProcessColor
        {
            get; set;
        }
    }

    /// <summary>
    /// 接收数据一般属性类
    /// </summary>
    public class WoInput : ComponentInput<WoStatusControl>
    {
        /// <summary>
        /// LineID
        /// </summary>
        [Description("产线ID")]
        [DisplayName("产线ID")]
        public virtual double LineID { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        [Description("班次ID")]
        [DisplayName("班次ID")]
        public virtual double ShiftId { get; set; }
    }
}