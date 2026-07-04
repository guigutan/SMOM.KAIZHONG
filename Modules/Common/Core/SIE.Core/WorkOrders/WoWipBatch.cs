using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.WorkOrders
{
    /// <summary>
    /// 生产批次
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("生产批次")]
    public partial class WoWipBatch : DataEntity
    {
        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        [MinValue(1)]
        public static readonly Property<decimal?> QtyProperty = P<WoWipBatch>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WoWipBatch>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Core.WorkOrders.WorkOrder> WorkOrderProperty = P<WoWipBatch>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public Core.WorkOrders.WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 生产批次 实体配置
    /// </summary>
    internal class WoWipBatchConfig : EntityConfig<WoWipBatch>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_WIP_BATCH").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(WoWipBatch.WorkOrderIdProperty).ColumnMeta.HasIndex();
        }
    }
}