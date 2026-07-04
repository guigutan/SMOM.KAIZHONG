using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 工单匹配物料继续使用数据
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单匹配物料")]
    public class CallMatchItem : DataEntity
    {
        #region 下一生产工单 WorkOrder
        /// <summary>
        /// 下一生产工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<CallMatchItem>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 下一生产工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 下一生产工单
        /// </summary>
        [Label("工单")]
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<CallMatchItem>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 下一生产工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<CallMatchItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<CallMatchItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 叫料工单 CallWorkOrder
        /// <summary>
        /// 叫料工单Id
        /// </summary>
        [Label("叫料工单")]
        public static readonly IRefIdProperty CallWorkOrderIdProperty = P<CallMatchItem>.RegisterRefId(e => e.CallWorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 叫料工单Id
        /// </summary>
        public double CallWorkOrderId
        {
            get { return (double)GetRefId(CallWorkOrderIdProperty); }
            set { SetRefId(CallWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 叫料工单
        /// </summary>
        [Label("叫料工单")]
        public static readonly RefEntityProperty<CallMaterialWorkOrder> CallWorkOrderProperty = P<CallMatchItem>.RegisterRef(e => e.CallWorkOrder, CallWorkOrderIdProperty);

        /// <summary>
        /// 叫料工单
        /// </summary>       
        public CallMaterialWorkOrder CallWorkOrder
        {
            get { return GetRefEntity(CallWorkOrderProperty); }
            set { SetRefEntity(CallWorkOrderProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单匹配物料 实体配置
    /// </summary>
    internal class CallMatchItemConfig : EntityConfig<CallMatchItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_MATCH_ITEM").MapAllProperties();
            Meta.Property(CallMatchItem.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
