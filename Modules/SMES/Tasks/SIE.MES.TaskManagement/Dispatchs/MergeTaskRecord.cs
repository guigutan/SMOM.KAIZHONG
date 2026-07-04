using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 合并任务记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("派工任务")]
    public class MergeTaskRecord : DataEntity
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<MergeTaskRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<MergeTaskRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 任务单 Task
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty TaskIdProperty =
            P<MergeTaskRecord>.RegisterRefId(e => e.TaskId, ReferenceType.Normal);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double TaskId
        {
            get { return (double)this.GetRefId(TaskIdProperty); }
            set { this.SetRefId(TaskIdProperty, value); }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> TaskProperty =
            P<MergeTaskRecord>.RegisterRef(e => e.Task, TaskIdProperty);

        /// <summary>
        /// 任务单
        /// </summary>
        public DispatchTask Task
        {
            get { return this.GetRefEntity(TaskProperty); }
            set { this.SetRefEntity(TaskProperty, value); }
        }
        #endregion

        #region 合并工单 MergeWorkOrder
        /// <summary>
        /// 合并工单
        /// </summary>
        [Label("合并工单")]
        [MaxLength(500)]
        public static readonly Property<string> MergeWorkOrderProperty = P<MergeTaskRecord>.Register(e => e.MergeWorkOrder);

        /// <summary>
        /// 合并工单
        /// </summary>
        public string MergeWorkOrder
        {
            get { return this.GetProperty(MergeWorkOrderProperty); }
            set { this.SetProperty(MergeWorkOrderProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 合并任务记录 实体配置
    /// </summary>
    internal class MergeTaskRecordEntityConfig : EntityConfig<MergeTaskRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_MERGE_TASK_RECORD").MapAllProperties();
            Meta.Property(MergeTaskRecord.MergeWorkOrderProperty).ColumnMeta.HasLength(1000);
            Meta.EnablePhantoms();
        }
    }
}