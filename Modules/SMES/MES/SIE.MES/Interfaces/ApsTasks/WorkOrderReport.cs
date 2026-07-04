using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 工单报工
    /// </summary>
    [RootEntity, Serializable]
    ////[CriteriaQuery]
    [Label("工单报工")]
    public partial class WorkOrderReport : DataEntity
    {
        #region 已报工最大工单统计ID StatisticId
        /// <summary>
        /// 已报工最大工单统计ID
        /// </summary>
        [Label("已报工最大工单统计ID")]
        public static readonly Property<double> StatisticIdProperty = P<WorkOrderReport>.Register(e => e.StatisticId);

        /// <summary>
        /// 已报工最大工单统计ID
        /// </summary>
        public double StatisticId
        {
            get { return GetProperty(StatisticIdProperty); }
            set { SetProperty(StatisticIdProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WorkOrderReport>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WorkOrderReport>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单报工实体配置类
    /// </summary>
    internal class WorkOrderReportConfig : EntityConfig<WorkOrderReport>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WO_REPORT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}