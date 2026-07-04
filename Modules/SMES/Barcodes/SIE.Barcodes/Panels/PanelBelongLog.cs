using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Barcodes.Panels
{
    /// <summary>
    /// 拼板码归属日志
    /// </summary>
    [RootEntity, Serializable]
    [Label("拼板码归属日志")]
    public partial class PanelBelongLog : DataEntity
    {
        #region 条码号 Sn
        /// <summary>
        /// 条码号
        /// </summary>
        [Label("条码号")]
        public static readonly Property<string> SnProperty = P<PanelBelongLog>.Register(e => e.Sn);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<PanelBelongLog>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<PanelBelongLog>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 原工单 OrgWorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty OrgWorkOrderIdProperty = P<PanelBelongLog>.RegisterRefId(e => e.OrgWorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double OrgWorkOrderId
        {
            get { return (double)GetRefId(OrgWorkOrderIdProperty); }
            set { SetRefId(OrgWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> OrgWorkOrderProperty = P<PanelBelongLog>.RegisterRef(e => e.OrgWorkOrder, OrgWorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder OrgWorkOrder
        {
            get { return GetRefEntity(OrgWorkOrderProperty); }
            set { SetRefEntity(OrgWorkOrderProperty, value); }
        }
        #endregion

        #region 操作人 Operator
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperatorIdProperty = P<PanelBelongLog>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> OperatorProperty = P<PanelBelongLog>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operator
        {
            get { return GetRefEntity(OperatorProperty); }
            set { SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 操作日期 OperatDate
        /// <summary>
        /// 操作日期
        /// </summary>
        [Label("操作日期")]
        public static readonly Property<DateTime> OperatDateProperty = P<PanelBelongLog>.Register(e => e.OperatDate);

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime OperatDate
        {
            get { return GetProperty(OperatDateProperty); }
            set { SetProperty(OperatDateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 拼板码归属日志 实体配置
    /// </summary>
    internal class PanelBelongLogConfig : EntityConfig<PanelBelongLog>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PN_BELONG_LOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
