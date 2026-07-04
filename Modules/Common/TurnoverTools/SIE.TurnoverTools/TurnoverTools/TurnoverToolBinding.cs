using SIE.Core.Barcodes;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 周转工具绑定明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("周转工具绑定明细")]
    public partial class TurnoverToolBinding : DataEntity
    {
        #region 产品条码 Sn
        /// <summary>
        /// 产品条码
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Label("产品条码")]
        public static readonly Property<string> SnProperty = P<TurnoverToolBinding>.Register(e => e.Sn);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<TurnoverToolBinding>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 是否已解绑 IsUnbinding
        /// <summary>
        /// 是否已解绑
        /// </summary>
        [Label("是否已解绑")]
        public static readonly Property<bool> IsUnbindingProperty = P<TurnoverToolBinding>.Register(e => e.IsUnbinding);

        /// <summary>
        /// 是否已解绑
        /// </summary>
        public bool IsUnbinding
        {
            get { return GetProperty(IsUnbindingProperty); }
            set { SetProperty(IsUnbindingProperty, value); }
        }
        #endregion

        #region 绑定时间 BindingDate
        /// <summary>
        /// 绑定时间
        /// </summary>
        [Label("绑定时间")]
        public static readonly Property<DateTime> BindingDateProperty = P<TurnoverToolBinding>.Register(e => e.BindingDate);

        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime BindingDate
        {
            get { return GetProperty(BindingDateProperty); }
            set { SetProperty(BindingDateProperty, value); }
        }
        #endregion

        #region 解绑时间 UnBindingDate
        /// <summary>
        /// 解绑时间
        /// </summary>
        [Label("解绑时间")]
        public static readonly Property<DateTime?> UnBindingDateProperty = P<TurnoverToolBinding>.Register(e => e.UnBindingDate);

        /// <summary>
        /// 解绑时间
        /// </summary>
        public DateTime? UnBindingDate
        {
            get { return GetProperty(UnBindingDateProperty); }
            set { SetProperty(UnBindingDateProperty, value); }
        }
        #endregion

        #region 绑定操作员 BindingOperator
        /// <summary>
        /// 绑定操作员Id
        /// </summary>
        public static readonly IRefIdProperty BindingOperatorIdProperty = P<TurnoverToolBinding>.RegisterRefId(e => e.BindingOperatorId, ReferenceType.Normal);

        /// <summary>
        /// 绑定操作员Id
        /// </summary>
        public double BindingOperatorId
        {
            get { return (double)GetRefId(BindingOperatorIdProperty); }
            set { SetRefId(BindingOperatorIdProperty, value); }
        }

        /// <summary>
        /// 绑定操作员
        /// </summary>
        public static readonly RefEntityProperty<Employee> BindingOperatorProperty = P<TurnoverToolBinding>.RegisterRef(e => e.BindingOperator, BindingOperatorIdProperty);

        /// <summary>
        /// 绑定操作员
        /// </summary>
        public Employee BindingOperator
        {
            get { return GetRefEntity(BindingOperatorProperty); }
            set { SetRefEntity(BindingOperatorProperty, value); }
        }
        #endregion

        #region 解绑操作员 UnbindingOperator
        /// <summary>
        /// 解绑操作员Id
        /// </summary>
        public static readonly IRefIdProperty UnbindingOperatorIdProperty = P<TurnoverToolBinding>.RegisterRefId(e => e.UnbindingOperatorId, ReferenceType.Normal);

        /// <summary>
        /// 解绑操作员Id
        /// </summary>
        public double? UnbindingOperatorId
        {
            get { return (double?)GetRefNullableId(UnbindingOperatorIdProperty); }
            set { SetRefNullableId(UnbindingOperatorIdProperty, value); }
        }

        /// <summary>
        /// 解绑操作员
        /// </summary>
        public static readonly RefEntityProperty<Employee> UnbindingOperatorProperty = P<TurnoverToolBinding>.RegisterRef(e => e.UnbindingOperator, UnbindingOperatorIdProperty);

        /// <summary>
        /// 解绑操作员
        /// </summary>
        public Employee UnbindingOperator
        {
            get { return GetRefEntity(UnbindingOperatorProperty); }
            set { SetRefEntity(UnbindingOperatorProperty, value); }
        }
        #endregion

        #region 周转工具 TurnoverTool
        /// <summary>
        /// 周转工具Id
        /// </summary>
        public static readonly IRefIdProperty TurnoverToolIdProperty = P<TurnoverToolBinding>.RegisterRefId(e => e.TurnoverToolId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<TurnoverTool> TurnoverToolProperty = P<TurnoverToolBinding>.RegisterRef(e => e.TurnoverTool, TurnoverToolIdProperty);

        /// <summary>
        /// 周转工具
        /// </summary>
        public TurnoverTool TurnoverTool
        {
            get { return GetRefEntity(TurnoverToolProperty); }
            set { SetRefEntity(TurnoverToolProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<TurnoverToolBinding>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<TurnoverToolBinding>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly IRefIdProperty WorkOrderIdProperty = P<TurnoverToolBinding>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<TurnoverToolBinding>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 是否绑定完成 IsBindFinish
        /// <summary>
        /// 是否绑定完成
        /// </summary>
        [Label("是否绑定完成")]
        public static readonly Property<bool> IsBindFinishProperty = P<TurnoverToolBinding>.Register(e => e.IsBindFinish);

        /// <summary>
        /// 是否绑定完成
        /// </summary>
        public bool IsBindFinish
        {
            get { return this.GetProperty(IsBindFinishProperty); }
            set { this.SetProperty(IsBindFinishProperty, value); }
        }
        #endregion

        #region 条码类型 BarcodeType
        /// <summary>
        /// 条码类型    
        /// </summary>
        [Label("条码类型")]
        public static readonly Property<BarcodeType> BarcodeTypeProperty = P<TurnoverToolBinding>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType
        {
            get { return this.GetProperty(BarcodeTypeProperty); }
            set { this.SetProperty(BarcodeTypeProperty, value); }
        }
        #endregion


        #region 注册视图

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<TurnoverToolBinding>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<TurnoverToolBinding>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 周转工具编码 TurnoverToolCode
        /// <summary>
        /// 周转工具编码
        /// </summary>
        [Label("周转工具编码")]
        public static readonly Property<string> TurnoverToolCodeProperty = P<TurnoverToolBinding>.RegisterView(e => e.TurnoverToolCode, p => p.TurnoverTool.Code);

        /// <summary>
        /// 周转工具编码
        /// </summary>
        public string TurnoverToolCode
        {
            get { return this.GetProperty(TurnoverToolCodeProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<TurnoverToolBinding>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

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
    /// 绑定明细 实体配置
    /// </summary>
    internal class TurnoverToolBindingConfig : EntityConfig<TurnoverToolBinding>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_TURN_TM_BD").MapAllProperties();
            Meta.Property(TurnoverToolBinding.SnProperty).ColumnMeta.HasIndex();
            Meta.Property(TurnoverToolBinding.SnProperty).ColumnMeta.HasLength(200);
            Meta.EnablePhantoms();
        }
    }
}