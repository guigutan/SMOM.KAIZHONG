using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.ProcessTaskLists
{

    /// <summary>
    /// 工序任务清单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProcessTaskListCriteria))]
    
    [Label("工序任务清单")]
    public class ProcessTaskListViewModel : ViewModel
    {

        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<ProcessTaskListViewModel>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion


        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProcessTaskListViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProcessTaskListViewModel>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessTaskListViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<ProcessTaskListViewModel>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 资源 ResourcepName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourcepNameProperty = P<ProcessTaskListViewModel>.Register(e => e.ResourcepName);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourcepName
        {
            get { return this.GetProperty(ResourcepNameProperty); }
            set { SetProperty(ResourcepNameProperty, value); }
        }
        #endregion

        #region 工单状态 WoState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> WoStateProperty = P<ProcessTaskListViewModel>.Register(e => e.WoState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState WoState
        {
            get { return this.GetProperty(WoStateProperty); }
            set { SetProperty(WoStateProperty, value); }
        }
        #endregion

        #region 计划数量 WorkOrderPlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> WorkOrderPlanQtyProperty = P<ProcessTaskListViewModel>.Register(e => e.WorkOrderPlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal WorkOrderPlanQty
        {
            get { return this.GetProperty(WorkOrderPlanQtyProperty); }
            set { SetProperty(WorkOrderPlanQtyProperty, value); }
        }
        #endregion

        #region 已生成任务单数 TasksGeneratedQty
        /// <summary>
        /// 已生成任务单数
        /// </summary>
        [Label("已生成任务单数")]
        public static readonly Property<decimal> TasksGeneratedQtyProperty = P<ProcessTaskListViewModel>.Register(e => e.TasksGeneratedQty);

        /// <summary>
        /// 已生成任务单数
        /// </summary>
        public decimal TasksGeneratedQty
        {
            get { return this.GetProperty(TasksGeneratedQtyProperty); }
            set { SetProperty(TasksGeneratedQtyProperty, value); }
        }
        #endregion

        #region 已派工数量 SendQty
        /// <summary>
        /// 已派工数量
        /// </summary>
        [Label("已派工数量")]
        public static readonly Property<decimal> SendQtyProperty = P<ProcessTaskListViewModel>.Register(e => e.SendQty);

        /// <summary>
        /// 已派工数量
        /// </summary>
        public decimal SendQty
        {
            get { return GetProperty(SendQtyProperty); }
            set { SetProperty(SendQtyProperty, value); }
        }
        #endregion


        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<ProcessTaskListViewModel>.Register(e => e.PlanBeginDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndDate
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<ProcessTaskListViewModel>.Register(e => e.PlanEndDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion


        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<ProcessTaskListViewModel>.Register(e => e.FactoryName);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
            set { SetProperty(FactoryNameProperty, value); }

        }
        #endregion
    }
}
