using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.ProcessPrepareRecords
{
    /// <summary>
    /// 工序产前准备记录
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(ProcessPrepareRecordConfig))]
    [ConditionQueryType(typeof(ProcessPrepareRecordCriteria))]
    [Label("工序产前准备记录")]
    public class ProcessPrepareRecord : SIE.MES.ProcessPrepareRecords.ProcessPrepareRecord
    {
        #region 任务单 DispatchTask
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<ProcessPrepareRecord>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return (double?)this.GetRefNullableId(DispatchTaskIdProperty); }
            set { this.SetRefNullableId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<ProcessPrepareRecord>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 产前准备记录明细 PrepareRecordDetail
        /// <summary>
        /// 产前准备记录子表
        /// </summary>
        [Label("产前准备记录明细")]
        public static readonly ListProperty<EntityList<ProcessPrepareRecordDetail>> PrepareRecordDetailProperty = P<ProcessPrepareRecord>.RegisterList(e => e.PrepareRecordDetail);

        /// <summary>
        /// 产前准备记录子表
        /// </summary>
        public EntityList<ProcessPrepareRecordDetail> PrepareRecordDetail
        {
            get { return this.GetLazyList(PrepareRecordDetailProperty); }
        }
        #endregion

        #region 视图属性

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double?> WorkOrderIdProperty = P<ProcessPrepareRecord>.RegisterView(e => e.WorkOrderId, p => p.DispatchTask.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ProcessPrepareRecord>.RegisterView(e => e.WorkOrderNo, p => p.DispatchTask.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 产品类型 ProductType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Label("产品类型")]
        public static readonly Property<ItemType> ProductTypeProperty = P<ProcessPrepareRecord>.RegisterView(e => e.ProductType, e => e.DispatchTask.WorkOrder.Product.Type);

        /// <summary>
        /// 产品类型
        /// </summary>
        public ItemType ProductType
        {
            get { return GetProperty(ProductTypeProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProcessPrepareRecord>.RegisterView(e => e.ProductCode, e => e.DispatchTask.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProcessPrepareRecord>.RegisterView(e => e.ProductName, e => e.DispatchTask.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 扩展属性值 ItemExtPropName
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<ProcessPrepareRecord>.RegisterView(e => e.ItemExtPropName, p => p.DispatchTask.WorkOrder.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> StateProperty = P<ProcessPrepareRecord>.RegisterView(e => e.State, p => p.DispatchTask.WorkOrder.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState State
        {
            get { return GetProperty(StateProperty); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<ProcessPrepareRecord>.RegisterView(e => e.PlanQty, p => p.DispatchTask.WorkOrder.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<ProcessPrepareRecord>.RegisterView(e => e.Factory, p => p.DispatchTask.WorkOrder.Factory.Name);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopProperty = P<ProcessPrepareRecord>.RegisterView(e => e.WorkShop, p => p.DispatchTask.WorkOrder.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop
        {
            get { return this.GetProperty(WorkShopProperty); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceProperty = P<ProcessPrepareRecord>.RegisterView(e => e.Resource, p => p.DispatchTask.Resource.Code);

        /// <summary>
        /// 资源
        /// </summary>
        public string Resource
        {
            get { return this.GetProperty(ResourceProperty); }
        }
        #endregion

        #region 资源Id ResourceId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double?> ResourceIdProperty = P<ProcessPrepareRecord>.RegisterView(e => e.ResourceId, p => p.DispatchTask.ResourceId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime?> PlanBeginDateProperty = P<ProcessPrepareRecord>.RegisterView(e => e.PlanBeginDate, p => p.DispatchTask.WorkOrder.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
        }
        #endregion

        #region 工单类型 Type
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> TypeProperty = P<ProcessPrepareRecord>.RegisterView(e => e.Type, p => p.DispatchTask.WorkOrder.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType Type
        {
            get { return GetProperty(TypeProperty); }
        }
        #endregion

        #region 派工单号 DispatchTaskNo
        /// <summary>
        /// 派工单号
        /// </summary>
        [Label("派工单号")]
        public static readonly Property<string> DispatchTaskNoProperty = P<ProcessPrepareRecord>.RegisterView(e => e.DispatchTaskNo, p => p.DispatchTask.No);

        /// <summary>
        /// 派工单号
        /// </summary>
        public string DispatchTaskNo
        {
            get { return this.GetProperty(DispatchTaskNoProperty); }
        }
        #endregion

        #region 派工单状态 TaskStatus
        /// <summary>
        /// 派工单状态
        /// </summary>
        [Label("派工单状态")]
        public static readonly Property<DispatchTaskStatus?> TaskStatusProperty = P<ProcessPrepareRecord>.RegisterView(e => e.TaskStatus, p => p.DispatchTask.TaskStatus);

        /// <summary>
        /// 派工单状态
        /// </summary>
        public DispatchTaskStatus? TaskStatus
        {
            get { return this.GetProperty(TaskStatusProperty); }
        }
        #endregion

        #region 派工任务执行对象 TaskPerformer
        /// <summary>
        /// 派工任务执行对象
        /// </summary>
        [Label("派工任务执行对象")]
        public static readonly Property<string> TaskPerformerProperty = P<ProcessPrepareRecord>.RegisterView(e => e.TaskPerformer, p => p.DispatchTask.TaskPerformer);

        /// <summary>
        /// 派工任务执行对象
        /// </summary>
        public string TaskPerformer
        {
            get { return this.GetProperty(TaskPerformerProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProcessPrepareRecord>.RegisterView(e => e.ProcessCode, p => p.DispatchTask.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessPrepareRecord>.RegisterView(e => e.ProcessName, p => p.DispatchTask.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #endregion
    }
}
