using SIE.Domain;
using SIE.Core.WorkOrders;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [RootEntity, Serializable]
    [Label("操作日志")]
    public partial class TurnoverToolActionLog : DataEntity
    {
        #region 周转工具 TurnoverTool
        /// <summary>
        /// 周转工具Id
        /// </summary>
        public static readonly IRefIdProperty TurnoverToolIdProperty = P<TurnoverToolActionLog>.RegisterRefId(e => e.TurnoverToolId, ReferenceType.Normal);

        /// <summary>
        /// 周转工具Id
        /// </summary>
        public double TurnoverToolId
        {
            get { return (double)GetRefId(TurnoverToolIdProperty); }
            set { SetRefId(TurnoverToolIdProperty, value); }
        }

        /// <summary>
        /// 周转工具
        /// </summary>
        public static readonly RefEntityProperty<TurnoverTool> TurnoverToolProperty = P<TurnoverToolActionLog>.RegisterRef(e => e.TurnoverTool, TurnoverToolIdProperty);

        /// <summary>
        /// 周转工具
        /// </summary>
        public TurnoverTool TurnoverTool
        {
            get { return GetRefEntity(TurnoverToolProperty); }
            set { SetRefEntity(TurnoverToolProperty, value); }
        }
        #endregion

        #region 周转工具操作 TurnoverToolAction
        /// <summary>
        /// 周转工具操作
        /// </summary>
        [Label("周转工具操作")]
        public static readonly Property<TurnoverToolAction> TurnoverToolActionProperty = P<TurnoverToolActionLog>.Register(e => e.TurnoverToolAction);

        /// <summary>
        /// 周转工具操作
        /// </summary>
        public TurnoverToolAction TurnoverToolAction
        {
            get { return GetProperty(TurnoverToolActionProperty); }
            set { SetProperty(TurnoverToolActionProperty, value); }
        }
        #endregion

        #region 产品条码 Sn
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        [MaxLength(50)]
        public static readonly Property<string> SnProperty = P<TurnoverToolActionLog>.Register(e => e.Sn);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal?> QtyProperty = P<TurnoverToolActionLog>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<TurnoverToolActionLog>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<TurnoverToolActionLog>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<TurnoverToolActionLog>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<TurnoverToolActionLog>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<TurnoverToolActionLog>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<TurnoverToolActionLog>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<TurnoverToolActionLog>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 操作日志 实体配置
    /// </summary>
    internal class TurnoverToolActionLogConfig : EntityConfig<TurnoverToolActionLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_TURN_TM_LOG").MapAllProperties();
            Meta.Property(TurnoverToolActionLog.SnProperty).ColumnMeta.HasLength(200);
            Meta.EnablePhantoms();
        }
    }
}