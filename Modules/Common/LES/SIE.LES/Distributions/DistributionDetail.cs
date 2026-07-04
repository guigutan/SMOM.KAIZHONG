using SIE.Domain;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
namespace SIE.LES.Distributions
{
    /// <summary>
    /// 配送单管理
    /// </summary>
    [ChildEntity, Serializable]   
    [Label("配送单管理明细")]
    public class DistributionDetail : DataEntity     
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<DistributionDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<DistributionDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<DistributionDetail>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion         

        #region 批次 LotCode
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotCodeProperty = P<DistributionDetail>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 分配ID AssignId
        /// <summary>
        /// 分配ID
        /// </summary>
        [Label("分配ID")]
        public static readonly Property<string> AssignIdProperty = P<DistributionDetail>.Register(e => e.AssignId);

        /// <summary>
        /// 分配ID
        /// </summary>
        public string AssignId
        {
            get { return this.GetProperty(AssignIdProperty); }
            set { this.SetProperty(AssignIdProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<DistributionDetail>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<DistributionDetail>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 库存状态 OnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState> OnhandStateProperty = P<DistributionDetail>.Register(e => e.OnhandState);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState OnhandState
        {
            get { return this.GetProperty(OnhandStateProperty); }
            set { this.SetProperty(OnhandStateProperty, value); }
        }
        #endregion

        #region 配送管理 Distribution
        /// <summary>
        /// 配送管理Id
        /// </summary>
        [Label("配送管理")]
        public static readonly IRefIdProperty DistributionIdProperty =
            P<DistributionDetail>.RegisterRefId(e => e.DistributionId, ReferenceType.Parent);

        /// <summary>
        /// 配送管理Id
        /// </summary>
        public double DistributionId
        {
            get { return (double)this.GetRefId(DistributionIdProperty); }
            set { this.SetRefId(DistributionIdProperty, value); }
        }

        /// <summary>
        /// 配送管理
        /// </summary>
        public static readonly RefEntityProperty<Distribution> DistributionProperty =
            P<DistributionDetail>.RegisterRef(e => e.Distribution, DistributionIdProperty);

        /// <summary>
        /// 配送管理
        /// </summary>
        public Distribution Distribution
        {
            get { return this.GetRefEntity(DistributionProperty); }
            set { this.SetRefEntity(DistributionProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<DistributionDetail>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 发运单行号 SoLineNo
        /// <summary>
        /// 发运单行号
        /// </summary>
        [Label("发运单行号")]
        public static readonly Property<string> SoLineNoProperty = P<DistributionDetail>.Register(e => e.SoLineNo);

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo
        {
            get { return this.GetProperty(SoLineNoProperty); }
            set { this.SetProperty(SoLineNoProperty, value); }
        }
        #endregion

        #region 备料单号 OrderNo
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> OrderNoProperty = P<DistributionDetail>.Register(e => e.OrderNo);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string OrderNo
        {
            get { return this.GetProperty(OrderNoProperty); }
            set { this.SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 备料单行号 OrderLineNo
        /// <summary>
        /// 备料单行号
        /// </summary>
        [Label("备料单行号")]
        public static readonly Property<string> OrderLineNoProperty = P<DistributionDetail>.Register(e => e.OrderLineNo);

        /// <summary>
        /// 备料单行号
        /// </summary>
        public string OrderLineNo
        {
            get { return this.GetProperty(OrderLineNoProperty); }
            set { this.SetProperty(OrderLineNoProperty, value); }
        }
        #endregion

        #region 发运单明细Id SoDtlId
        /// <summary>
        /// 发运单明细Id
        /// </summary>
        [Label("发运单明细Id")]
        public static readonly Property<double> SoDtlIdProperty = P<DistributionDetail>.Register(e => e.SoDtlId);

        /// <summary>
        /// 发运单明细Id
        /// </summary>
        public double SoDtlId
        {
            get { return this.GetProperty(SoDtlIdProperty); }
            set { this.SetProperty(SoDtlIdProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<DistributionDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<DistributionDetail>.RegisterView(e => e.UnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<DistributionDetail>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 规格型号 ItemSpec
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecProperty = P<DistributionDetail>.RegisterView(e => e.ItemSpec, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpec
        {
            get { return this.GetProperty(ItemSpecProperty); }
        }
        #endregion

        #region 状态 OrderState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OrderState> OrderStateProperty = P<DistributionDetail>.RegisterView(e => e.OrderState, p => p.Distribution.OrderState);

        /// <summary>
        /// 状态
        /// </summary>
        public OrderState OrderState
        {
            get { return this.GetProperty(OrderStateProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 配送单管理 实体配置
    /// </summary>
    internal class DistributionDetailConfig : EntityConfig<DistributionDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DISTRIBUTION_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
