using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.DIST
{
    /// <summary>
    /// 配送管理
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("配送管理")]
    [DisplayMember(nameof(GoodsIssue.SendNo))]
    public partial class GoodsIssue : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GoodsIssue()
        {
            DistributionQty = 0m;
            RemainderQty = 0m;
            DefectReturnQty = 0m;
            NormalReturnQty = 0m;
            DefectQty = 0;
        }

        #region 发货数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        [Label("发货数量")]
        public static readonly Property<decimal?> QtyProperty = P<GoodsIssue>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainderQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> RemainderQtyProperty = P<GoodsIssue>.Register(e => e.RemainderQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainderQty
        {
            get { return this.GetProperty(RemainderQtyProperty); }
            set { this.SetProperty(RemainderQtyProperty, value); }
        }
        #endregion

        #region 配送数量 DistributionQty
        /// <summary>
        /// 配送数量
        /// </summary>
        [Label("配送数量")]
        public static readonly Property<decimal> DistributionQtyProperty = P<GoodsIssue>.Register(e => e.DistributionQty);

        /// <summary>
        /// 配送数量
        /// </summary>
        public decimal DistributionQty
        {
            get { return this.GetProperty(DistributionQtyProperty); }
            set { this.SetProperty(DistributionQtyProperty, value); }
        }
        #endregion

        #region 缺陷退货数量 DefectReturnQty
        /// <summary>
        /// 缺陷退货数量
        /// </summary>
        [Required]
        [Label("缺陷退货数量")]
        public static readonly Property<decimal> DefectReturnQtyProperty = P<GoodsIssue>.Register(e => e.DefectReturnQty);

        /// <summary>
        /// 缺陷退货数量
        /// </summary>
        public decimal DefectReturnQty
        {
            get { return GetProperty(DefectReturnQtyProperty); }
            set { SetProperty(DefectReturnQtyProperty, value); }
        }
        #endregion

        #region 正常退货数量 NormalReturnQty
        /// <summary>
        /// 正常退货数量
        /// </summary>
        [Required]
        [Label("正常退货数量")]
        public static readonly Property<decimal> NormalReturnQtyProperty = P<GoodsIssue>.Register(e => e.NormalReturnQty);

        /// <summary>
        /// 正常退货数量
        /// </summary>
        public decimal NormalReturnQty
        {
            get { return GetProperty(NormalReturnQtyProperty); }
            set { SetProperty(NormalReturnQtyProperty, value); }
        }
        #endregion

        #region 缺陷数量 DefectQty
        /// <summary>
        /// 缺陷数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("缺陷数量")]
        public static readonly Property<int> DefectQtyProperty = P<GoodsIssue>.Register(e => e.DefectQty);

        /// <summary>
        /// 缺陷数量
        /// </summary>
        public int DefectQty
        {
            get { return GetProperty(DefectQtyProperty); }
            set { SetProperty(DefectQtyProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty =
            P<GoodsIssue>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位
        /// </summary>
        public double UnitId
        {
            get { return (double)this.GetRefId(UnitIdProperty); }
            set { this.SetRefId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty =
            P<GoodsIssue>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 发货单号 SendNo
        /// <summary>
        /// 发货单号
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("发货单号")]
        public static readonly Property<string> SendNoProperty = P<GoodsIssue>.Register(e => e.SendNo);

        /// <summary>
        /// 发货单号
        /// </summary>
        public string SendNo
        {
            get { return GetProperty(SendNoProperty); }
            set { SetProperty(SendNoProperty, value); }
        }
        #endregion

        #region 批号 BatchNo
        /// <summary>
        /// 批号
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("批号")]
        public static readonly Property<string> BatchNoProperty = P<GoodsIssue>.Register(e => e.BatchNo);

        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary> 
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<GoodsIssue>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<GoodsIssue>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
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
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<GoodsIssue>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<GoodsIssue>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 发料属性 PropertyValueList
        /// <summary>
        /// 发料属性
        /// </summary>
        public static readonly ListProperty<EntityList<GoodsIssuePropertyValue>> PropertyValueListProperty = P<GoodsIssue>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 发料属性
        /// </summary>
        public EntityList<GoodsIssuePropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<GoodsIssue>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<GoodsIssue>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<GoodsIssue>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<GoodsIssue>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 物料规格 ItemModel
        /// <summary>
        /// 物料规格
        /// </summary>
        [Label("物料规格")]
        public static readonly Property<string> ItemModelProperty = P<GoodsIssue>.RegisterView(e => e.ItemModel, p => p.Item.Model.Name);

        /// <summary>
        /// 物料规格
        /// </summary>
        public string ItemModel
        {
            get { return this.GetProperty(ItemModelProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 配送管理 实体配置
    /// </summary>
    internal class GoodsIssueConfig : EntityConfig<GoodsIssue>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_GOODS_ISSUE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}