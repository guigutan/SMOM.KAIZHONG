using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单日志
    /// </summary>
    [ChildEntity, Serializable]
    ////[ConditionQueryType(typeof(WorkOrderLogCriteria))]
    [Label("工单日志")]
    public partial class WorkOrderLog : DataEntity
    {
        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        public static readonly Property<string> ReasonProperty = P<WorkOrderLog>.Register(e => e.Reason);

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get { return GetProperty(ReasonProperty); }
            set { SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 操作时间 OperatDate
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OperatDateProperty = P<WorkOrderLog>.Register(e => e.OperatDate);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatDate
        {
            get { return GetProperty(OperatDateProperty); }
            set { SetProperty(OperatDateProperty, value); }
        }
        #endregion

        #region 操作类型 Type
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<WorkOrderLogType> TypeProperty = P<WorkOrderLog>.Register(e => e.Type);

        /// <summary>
        /// 操作类型
        /// </summary>
        public WorkOrderLogType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 操作人 Operator
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperatorIdProperty = P<WorkOrderLog>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperatorId
        {
            get { return (double)GetRefId(OperatorIdProperty); }
            set { SetRefId(OperatorIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        [Label("操作人")]
        public static readonly RefEntityProperty<Employee> OperatorProperty = P<WorkOrderLog>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operator
        {
            get { return GetRefEntity(OperatorProperty); }
            set { SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 工单与日志关系 WorkOrder
        /// <summary>
        /// 工单与日志关系Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WorkOrderLog>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 工单与日志关系Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单与日志关系
        /// </summary>
        [Label("工单")]
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WorkOrderLog>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单与日志关系
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单日志 实体配置
    /// </summary>
    internal class WorkOrderLogConfig : EntityConfig<WorkOrderLog>
    {
        /// <summary>
        /// 实体数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_LOG").MapAllProperties();
            Meta.Property(WorkOrderLog.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}