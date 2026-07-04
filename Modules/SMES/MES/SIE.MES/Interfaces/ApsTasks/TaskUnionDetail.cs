using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 计划任务关联明细
    /// </summary>
    [ChildEntity, Serializable]
    ////[CriteriaQuery]
    [Label("计划任务关联明细")]
    public partial class TaskUnionDetail : DataEntity
    {
        #region 明细Id DetailId
        /// <summary>
        /// 明细Id
        /// </summary>
        [Label("明细Id")]
        public static readonly Property<string> DetailIdProperty = P<TaskUnionDetail>.Register(e => e.DetailId);

        /// <summary>
        /// 明细Id
        /// </summary>
        public string DetailId
        {
            get { return GetProperty(DetailIdProperty); }
            set { SetProperty(DetailIdProperty, value); }
        }
        #endregion

        #region 客户Id CustomerId
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户Id")]
        public static readonly Property<double?> CustomerIdProperty = P<TaskUnionDetail>.Register(e => e.CustomerId);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return GetProperty(CustomerIdProperty); }
            set { SetProperty(CustomerIdProperty, value); }
        }
        #endregion

        #region 工艺单编号 ProcessTechOrderCode
        /// <summary>
        /// 工艺单编号
        /// </summary>
        [Label("工艺单编号")]
        [MaxLength(2000)]
        public static readonly Property<string> ProcessTechOrderCodeProperty = P<TaskUnionDetail>.Register(e => e.ProcessTechOrderCode);

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string ProcessTechOrderCode
        {
            get { return GetProperty(ProcessTechOrderCodeProperty); }
            set { SetProperty(ProcessTechOrderCodeProperty, value); }
        }
        #endregion

        #region 生产订单编号 ProductionOrderNo
        /// <summary>
        /// 生产订单编号
        /// </summary>
        [Label("生产订单编号")]
        [MaxLength(2000)]
        public static readonly Property<string> ProductionOrderNoProperty = P<TaskUnionDetail>.Register(e => e.ProductionOrderNo);

        /// <summary>
        /// 生产订单编号
        /// </summary>
        public string ProductionOrderNo
        {
            get { return GetProperty(ProductionOrderNoProperty); }
            set { SetProperty(ProductionOrderNoProperty, value); }
        }
        #endregion

        #region 是否主料 IsMainItem
        /// <summary>
        /// 是否主料
        /// </summary>
        [Label("是否主料")]
        public static readonly Property<bool> IsMainItemProperty = P<TaskUnionDetail>.Register(e => e.IsMainItem);

        /// <summary>
        /// 是否主料
        /// </summary>
        public bool IsMainItem
        {
            get { return GetProperty(IsMainItemProperty); }
            set { SetProperty(IsMainItemProperty, value); }
        }
        #endregion

        #region 累计报工数量TotalQty
        /// <summary>
        /// 累计报工数量
        /// </summary>
        [Label("累计报工数量")]
        public static readonly Property<decimal> TotalQtyProperty = P<TaskUnionDetail>.Register(e => e.TotalQty);

        /// <summary>
        /// 累计报工数量
        /// </summary>
        public decimal TotalQty
        {
            get { return GetProperty(TotalQtyProperty); }
            set { SetProperty(TotalQtyProperty, value); }
        }
        #endregion

        #region 是否全部报工 IsFinish
        /// <summary>
        /// 是否全部报工
        /// </summary>
        [Label("是否全部报工")]
        public static readonly Property<bool> IsFinishProperty = P<TaskUnionDetail>.Register(e => e.IsFinish);

        /// <summary>
        /// 是否全部报工
        /// </summary>
        public bool IsFinish
        {
            get { return GetProperty(IsFinishProperty); }
            set { SetProperty(IsFinishProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<TaskUnionDetail>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<TaskUnionDetail>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 计划任务关联 TaskUnion
        /// <summary>
        /// 计划任务关联Id
        /// </summary>
        public static readonly IRefIdProperty TaskUnionIdProperty = P<TaskUnionDetail>.RegisterRefId(e => e.TaskUnionId, ReferenceType.Parent);

        /// <summary>
        /// 计划任务关联Id
        /// </summary>
        public double TaskUnionId
        {
            get { return (double)GetRefId(TaskUnionIdProperty); }
            set { SetRefId(TaskUnionIdProperty, value); }
        }

        /// <summary>
        /// 计划任务关联
        /// </summary>
        public static readonly RefEntityProperty<TaskUnion> TaskUnionProperty = P<TaskUnionDetail>.RegisterRef(e => e.TaskUnion, TaskUnionIdProperty);

        /// <summary>
        /// 计划任务关联
        /// </summary>
        public TaskUnion TaskUnion
        {
            get { return GetRefEntity(TaskUnionProperty); }
            set { SetRefEntity(TaskUnionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 计划任务明细实体配置类
    /// </summary>
    internal class TaskUnionDetailConfig : EntityConfig<TaskUnionDetail>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TASK_UNION_DTL").MapAllProperties();
            Meta.Property(TaskUnionDetail.ProcessTechOrderCodeProperty).ColumnMeta.HasLength(2000);
            Meta.Property(TaskUnionDetail.ProductionOrderNoProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}