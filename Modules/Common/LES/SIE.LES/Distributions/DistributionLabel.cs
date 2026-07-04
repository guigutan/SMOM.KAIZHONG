using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.Distributions
{
    /// <summary>
    /// 配送单管理扫描记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("配送单管理扫描记录")]
    public class DistributionLabel : DataEntity
    {
        #region 配送管理 Distribution
        /// <summary>
        /// 配送管理Id
        /// </summary>
        [Label("配送管理")]
        public static readonly IRefIdProperty DistributionIdProperty =
            P<DistributionLabel>.RegisterRefId(e => e.DistributionId, ReferenceType.Parent);

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
            P<DistributionLabel>.RegisterRef(e => e.Distribution, DistributionIdProperty);

        /// <summary>
        /// 配送管理
        /// </summary>
        public Distribution Distribution
        {
            get { return this.GetRefEntity(DistributionProperty); }
            set { this.SetRefEntity(DistributionProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<DistributionLabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<DistributionLabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 标签号 LabelNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<DistributionLabel>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 最上级条码 HighestNo
        /// <summary>
        /// 最上级条码
        /// </summary>
        [Label("最上级条码")]
        public static readonly Property<string> HighestNoProperty = P<DistributionLabel>.Register(e => e.HighestNo);

        /// <summary>
        /// 最上级条码
        /// </summary>
        public string HighestNo
        {
            get { return this.GetProperty(HighestNoProperty); }
            set { this.SetProperty(HighestNoProperty, value); }
        }
        #endregion        

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<DistributionLabel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion         

        #region 是否序列 IsSerialNumber
        /// <summary>
        /// 是否序列
        /// </summary>
        [Label("是否序列")]
        public static readonly Property<bool> IsSerialNumberProperty = P<DistributionLabel>.Register(e => e.IsSerialNumber);

        /// <summary>
        /// 是否序列
        /// </summary>
        public bool IsSerialNumber
        {
            get { return this.GetProperty(IsSerialNumberProperty); }
            set { this.SetProperty(IsSerialNumberProperty, value); }
        }
        #endregion

        #region 配送明细 DistributionDetail
        /// <summary>
        /// 配送明细Id
        /// </summary>
        [Label("配送明细")]
        public static readonly IRefIdProperty DistributionDetailIdProperty =
            P<DistributionLabel>.RegisterRefId(e => e.DistributionDetailId, ReferenceType.Normal);

        /// <summary>
        /// 配送明细Id
        /// </summary>
        public double DistributionDetailId
        {
            get { return (double)this.GetRefId(DistributionDetailIdProperty); }
            set { this.SetRefId(DistributionDetailIdProperty, value); }
        }

        /// <summary>
        /// 配送明细
        /// </summary>
        public static readonly RefEntityProperty<DistributionDetail> DistributionDetailProperty =
            P<DistributionLabel>.RegisterRef(e => e.DistributionDetail, DistributionDetailIdProperty);

        /// <summary>
        /// 配送明细
        /// </summary>
        public DistributionDetail DistributionDetail
        {
            get { return this.GetRefEntity(DistributionDetailProperty); }
            set { this.SetRefEntity(DistributionDetailProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<DistributionLabel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<DistributionLabel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 分配Id AssignId
        /// <summary>
        /// 分配Id
        /// </summary>
        [Label("分配Id")]
        public static readonly Property<string> AssignIdProperty = P<DistributionLabel>.RegisterView(e => e.AssignId, p => p.DistributionDetail.AssignId);

        /// <summary>
        /// 分配Id
        /// </summary>
        public string AssignId
        {
            get { return this.GetProperty(AssignIdProperty); }
        }
        #endregion

        #region 发运单号码 OrderNo
        /// <summary>
        /// 发运单号码
        /// </summary>
        [Label("发运单号码")]
        public static readonly Property<string> OrderNoProperty = P<DistributionLabel>.RegisterView(e => e.OrderNo, p => p.Distribution.SourceNo);

        /// <summary>
        /// 发运单号码
        /// </summary>
        public string OrderNo
        {
            get { return this.GetProperty(OrderNoProperty); }
        }
        #endregion

        #region 发运单行号 OrderLineNo
        /// <summary>
        /// 发运单行号
        /// </summary>
        [Label("发运单行号")]
        public static readonly Property<string> OrderLineNoProperty = P<DistributionLabel>.RegisterView(e => e.OrderLineNo, p => p.DistributionDetail.SoLineNo);

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string OrderLineNo
        {
            get { return this.GetProperty(OrderLineNoProperty); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<DistributionLabel>.RegisterView(e => e.LotCode, p => p.DistributionDetail.LotCode);

        /// <summary>
        /// 行号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
        }
        #endregion

        #region 备料单号 StockOrderNo
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> StockOrderNoProperty = P<DistributionLabel>.RegisterView(e => e.StockOrderNo, p => p.DistributionDetail.OrderNo);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string StockOrderNo
        {
            get { return this.GetProperty(StockOrderNoProperty); }
        }
        #endregion

        #region 备料单行号 StockOrderLineNo
        /// <summary>
        /// 备料单行号
        /// </summary>
        [Label("备料单行号")]
        public static readonly Property<string> StockOrderLineNoProperty = P<DistributionLabel>.RegisterView(e => e.StockOrderLineNo, p => p.DistributionDetail.OrderLineNo);

        /// <summary>
        /// 备料单行号
        /// </summary>
        public string StockOrderLineNo
        {
            get { return this.GetProperty(StockOrderLineNoProperty); }
        }
        #endregion

        #region 仓库Id WarehouseId
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库Id")]
        public static readonly Property<double> WarehouseIdProperty = P<DistributionLabel>.RegisterView(e => e.WarehouseId, p => p.Distribution.WarehouseId);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
        }
        #endregion

        #region 配送单号 DistributionNo
        /// <summary>
        /// 配送单号
        /// </summary>
        [Label("配送单号")]
        public static readonly Property<string> DistributionNoProperty = P<DistributionLabel>.RegisterView(e => e.DistributionNo, p => p.Distribution.No);

        /// <summary>
        /// 配送单号
        /// </summary>
        public string DistributionNo
        {
            get { return this.GetProperty(DistributionNoProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 盘点单明细 实体配置
    /// </summary>
    internal class DistributionLabelConfig : EntityConfig<DistributionLabel>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("DIST_LABEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
