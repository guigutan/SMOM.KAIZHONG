using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.DIST
{
    /// <summary>
    /// 配送箱号
    /// </summary>
    [ChildEntity, Serializable]
    [Label("箱号")]
    [DisplayMember(nameof(ItemLabelNo))]
    public partial class DistributionBillDetail : DataEntity
    {
        #region 箱号条码号 ItemLabelNo
        /// <summary>
        /// 箱号条码号
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("箱号条码号")]
        public static readonly Property<string> ItemLabelNoProperty = P<DistributionBillDetail>.Register(e => e.ItemLabelNo);

        /// <summary>
        /// 箱号条码号
        /// </summary>
        public string ItemLabelNo
        {
            get { return GetProperty(ItemLabelNoProperty); }
            set { SetProperty(ItemLabelNoProperty, value); }
        }
        #endregion

        #region 装箱数量 Qty
        /// <summary>
        /// 装箱数量
        /// </summary>
        [Required]
        [Label("装箱数量")]
        public static readonly Property<decimal> QtyProperty = P<DistributionBillDetail>.Register(e => e.Qty);

        /// <summary>
        /// 装箱数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 良品数量 OkQty
        /// <summary>
        /// 良品数量
        /// </summary>
        [Required]
        [Label("良品数量")]
        public static readonly Property<decimal> OkQtyProperty = P<DistributionBillDetail>.Register(e => e.OkQty);

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal OkQty
        {
            get { return GetProperty(OkQtyProperty); }
            set { SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Required]
        [Label("不良品数量")]
        public static readonly Property<decimal> NgQtyProperty = P<DistributionBillDetail>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 配送属性 PropertyList
        /// <summary>
        /// 配送属性
        /// </summary>
        [Label("配送属性")]
        public static readonly ListProperty<EntityList<DistributionBillDetailProperty>> PropertyListProperty = P<DistributionBillDetail>.RegisterList(e => e.PropertyList);

        /// <summary>
        /// 配送属性
        /// </summary>
        public EntityList<DistributionBillDetailProperty> PropertyList
        {
            get { return this.GetLazyList(PropertyListProperty); }
        }
        #endregion

        #region 明细列表 Bill
        /// <summary>
        /// 明细列表Id
        /// </summary>
        public static readonly IRefIdProperty BillIdProperty = P<DistributionBillDetail>.RegisterRefId(e => e.BillId, ReferenceType.Parent);

        /// <summary>
        /// 明细列表Id
        /// </summary>
        public double BillId
        {
            get { return (double)GetRefId(BillIdProperty); }
            set { SetRefId(BillIdProperty, value); }
        }

        /// <summary>
        /// 明细列表
        /// </summary>
        public static readonly RefEntityProperty<DistributionBill> BillProperty = P<DistributionBillDetail>.RegisterRef(e => e.Bill, BillIdProperty);

        /// <summary>
        /// 明细列表
        /// </summary>
        public DistributionBill Bill
        {
            get { return GetRefEntity(BillProperty); }
            set { SetRefEntity(BillProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 箱号 实体配置
    /// </summary>
    internal class DistributionBillDetailConfig : EntityConfig<DistributionBillDetail>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_DIST_BILL_DTL").MapAllProperties();
            Meta.Property(DistributionBillDetail.BillIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}