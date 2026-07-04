using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 初始化默认批次信息(批次号不再唯一，需要通过批量号、物料、扩展属性ItemExtProp决定唯一)
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LotCriteria))]
    [DisplayMember(nameof(Lot.Code))]
    [Label("批次")]
    public partial class Lot : DataEntity
    {
        /// <summary>
        /// 默认批次编号（无批次管理）
        /// </summary>
        public const string LotDefault = "LotDefault";

        #region 批次 Code
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        [MaxLength(80)]        
        [Required]
        public static readonly Property<string> CodeProperty = P<Lot>.Register(e => e.Code);

        /// <summary>
        /// 批次
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        [MaxLength(80)]
        public static readonly Property<string> StorerCodeProperty = P<Lot>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<Lot>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<Lot>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<Lot>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary> 
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 批次属性01 LotAtt01
        /// <summary>
        /// 批次属性01
        /// </summary>
        [Label("批次属性01")]
        public static readonly Property<DateTime?> LotAtt01Property = P<Lot>.Register(e => e.LotAtt01);

        /// <summary>
        /// 批次属性01
        /// </summary>
        public DateTime? LotAtt01
        {
            get { return GetProperty(LotAtt01Property); }
            set { SetProperty(LotAtt01Property, value); }
        }
        #endregion

        #region 批次属性02 LotAtt02
        /// <summary>
        /// 批次属性02
        /// </summary>
        [Label("批次属性02")]
        public static readonly Property<DateTime?> LotAtt02Property = P<Lot>.Register(e => e.LotAtt02);

        /// <summary>
        /// 批次属性02
        /// </summary>
        public DateTime? LotAtt02
        {
            get { return GetProperty(LotAtt02Property); }
            set { SetProperty(LotAtt02Property, value); }
        }
        #endregion

        #region 批次属性03 LotAtt03
        /// <summary>
        /// 批次属性03
        /// </summary>
        [Label("批次属性03")]
        public static readonly Property<DateTime?> LotAtt03Property = P<Lot>.Register(e => e.LotAtt03);

        /// <summary>
        /// 批次属性03
        /// </summary>
        public DateTime? LotAtt03
        {
            get { return GetProperty(LotAtt03Property); }
            set { SetProperty(LotAtt03Property, value); }
        }
        #endregion

        #region 批次属性04 LotAtt04
        /// <summary>
        /// 批次属性04
        /// </summary>
        [MaxLength(80)]
        [Label("批次属性04")]
        public static readonly Property<string> LotAtt04Property = P<Lot>.Register(e => e.LotAtt04);

        /// <summary>
        /// 批次属性04
        /// </summary>
        public string LotAtt04
        {
            get { return GetProperty(LotAtt04Property); }
            set { SetProperty(LotAtt04Property, value); }
        }
        #endregion

        #region 批次属性05 LotAtt05
        /// <summary>
        /// 批次属性05
        /// </summary>
        [Label("批次属性05")]
        public static readonly Property<decimal?> LotAtt05Property = P<Lot>.Register(e => e.LotAtt05);

        /// <summary>
        /// 批次属性05
        /// </summary>
        public decimal? LotAtt05
        {
            get { return GetProperty(LotAtt05Property); }
            set { SetProperty(LotAtt05Property, value); }
        }
        #endregion

        #region 批次属性06 LotAtt06
        /// <summary>
        /// 批次属性06
        /// </summary>
        [Label("批次属性06")]
        public static readonly Property<decimal?> LotAtt06Property = P<Lot>.Register(e => e.LotAtt06);

        /// <summary>
        /// 批次属性06
        /// </summary>
        public decimal? LotAtt06
        {
            get { return GetProperty(LotAtt06Property); }
            set { SetProperty(LotAtt06Property, value); }
        }
        #endregion

        #region 是否特采 LotAtt07
        /// <summary>
        /// 是否特采
        /// </summary>
        [Label("是否特采")]
        public static readonly Property<bool?> LotAtt07Property = P<Lot>.Register(e => e.LotAtt07);

        /// <summary>
        /// 是否特采
        /// </summary>
        public bool? LotAtt07
        {
            get { return GetProperty(LotAtt07Property); }
            set { SetProperty(LotAtt07Property, value); }
        }
        #endregion

        #region 批次属性08 LotAtt08
        /// <summary>
        /// 批次属性08
        /// </summary>
        [Label("批次属性08")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt08Property = P<Lot>.Register(e => e.LotAtt08);

        /// <summary>
        /// 批次属性08
        /// </summary>
        public string LotAtt08
        {
            get { return GetProperty(LotAtt08Property); }
            set { SetProperty(LotAtt08Property, value); }
        }
        #endregion

        #region 批次属性09 LotAtt09
        /// <summary>
        /// 批次属性09
        /// </summary>
        [Label("批次属性09")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt09Property = P<Lot>.Register(e => e.LotAtt09);

        /// <summary>
        /// 批次属性09
        /// </summary>
        public string LotAtt09
        {
            get { return GetProperty(LotAtt09Property); }
            set { SetProperty(LotAtt09Property, value); }
        }
        #endregion

        #region 批次属性10 LotAtt10
        /// <summary>
        /// 批次属性10
        /// </summary>
        [Label("批次属性10")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt10Property = P<Lot>.Register(e => e.LotAtt10);

        /// <summary>
        /// 批次属性10
        /// </summary>
        public string LotAtt10
        {
            get { return GetProperty(LotAtt10Property); }
            set { SetProperty(LotAtt10Property, value); }
        }
        #endregion

        #region 批次属性11 LotAtt11
        /// <summary>
        /// 批次属性11
        /// </summary>
        [Label("批次属性11")]
        public static readonly Property<DateTime?> LotAtt11Property = P<Lot>.Register(e => e.LotAtt11);

        /// <summary>
        /// 批次属性11
        /// </summary>
        public DateTime? LotAtt11
        {
            get { return GetProperty(LotAtt11Property); }
            set { SetProperty(LotAtt11Property, value); }
        }
        #endregion

        #region 批次属性12 LotAtt12
        /// <summary>
        /// 批次属性12
        /// </summary>
        [Label("批次属性12")]
        public static readonly Property<DateTime?> LotAtt12Property = P<Lot>.Register(e => e.LotAtt12);

        /// <summary>
        /// 批次属性12
        /// </summary>
        public DateTime? LotAtt12
        {
            get { return GetProperty(LotAtt12Property); }
            set { SetProperty(LotAtt12Property, value); }
        }
        #endregion

        #region Asn单号 AsnNo
        /// <summary>
        /// Asn单号
        /// </summary>
        [Label("Asn单号")]
        public static readonly Property<string> AsnNoProperty = P<Lot>.Register(e => e.AsnNo);

        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnNo
        {
            get { return this.GetProperty(AsnNoProperty); }
            set { this.SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region 顺序号 SeqNo
        /// <summary>
        /// 顺序号
        /// </summary>
        [Label("顺序号")]
        public static readonly Property<int> SeqNoProperty = P<Lot>.Register(e => e.SeqNo);

        /// <summary>
        /// 顺序号
        /// </summary>
        public int SeqNo
        {
            get { return this.GetProperty(SeqNoProperty); }
            set { this.SetProperty(SeqNoProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<Lot>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<Lot>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<Lot>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<Lot>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }
        #endregion

        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<Lot>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性显示名 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<Lot>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty =
            P<Lot>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)this.GetRefNullableId(SupplierIdProperty); }
            set { this.SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty =
            P<Lot>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return this.GetRefEntity(SupplierProperty); }
            set { this.SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region ASN明细Id AsnDetailId
        /// <summary>
        /// ASN明细Id
        /// </summary>      
        public static readonly Property<double?> AsnDetailIdProperty = P<Lot>.Register(e => e.AsnDetailId);

        /// <summary>
        /// ASN明细Id
        /// </summary>
        public double? AsnDetailId
        {
            get { return this.GetProperty(AsnDetailIdProperty); }
            set { this.SetProperty(AsnDetailIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 批次 实体配置
    /// </summary>
    internal class LotConfig : EntityConfig<Lot>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LOT").MapAllProperties();
            Meta.Property(Lot.SeqNoProperty).DontMapColumn();
            Meta.Property(Lot.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.Property(Lot.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 批次扩展
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次扩展")]
    public partial class LotExt : Lot//!为了自定义查询块与列表块，多个join使用
    {
    }
}