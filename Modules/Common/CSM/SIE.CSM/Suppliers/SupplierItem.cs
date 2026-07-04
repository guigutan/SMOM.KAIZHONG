using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 供应商与物料关系
    /// </summary>
    [ChildEntity, Serializable]
    [Label("供应商与物料关系")]
    public partial class SupplierItem : DataEntity
    {
        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(2000)]
        public static readonly Property<string> RemarkProperty = P<SupplierItem>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 采购供应模式 PurchaseSupplyType
        /// <summary>
        /// 采购供应模式
        /// </summary>
        [Label("采购供应模式")]
        public static readonly Property<PurchaseSupplyType> PurchaseSupplyTypeProperty = P<SupplierItem>.Register(e => e.PurchaseSupplyType);

        /// <summary>
        /// 采购供应模式
        /// </summary>
        public PurchaseSupplyType PurchaseSupplyType
        {
            get { return GetProperty(PurchaseSupplyTypeProperty); }
            set { SetProperty(PurchaseSupplyTypeProperty, value); }
        }
        #endregion

        #region 供应商与物料关系 Supplier
        /// <summary>
        /// 供应商与物料关系Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<SupplierItem>.RegisterRefId(e => e.SupplierId, ReferenceType.Parent);

        /// <summary>
        /// 供应商与物料关系Id
        /// </summary>
        public double SupplierId
        {
            get { return (double)GetRefId(SupplierIdProperty); }
            set { SetRefId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商与物料关系
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SupplierItem>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商与物料关系
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 物料与供应商关系 Item
        /// <summary>
        /// 物料与供应商关系Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<SupplierItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料与供应商关系Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料与供应商关系
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<SupplierItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料与供应商关系
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region MPN MPN
        /// <summary>
        /// MPN
        /// </summary>
        [Label("MPN")]
        public static readonly Property<string> MPNProperty = P<SupplierItem>.Register(e => e.MPN);

        /// <summary>
        /// MPN
        /// </summary>
        public string MPN
        {
            get { return this.GetProperty(MPNProperty); }
            set { this.SetProperty(MPNProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<SupplierItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

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
        public static readonly Property<string> ItemCodeProperty = P<SupplierItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemDescriptionProperty = P<SupplierItem>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemDescriptionProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<SupplierItem>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 供应商与物料关系 实体配置
    /// </summary>
    internal class SupplierItemConfig : EntityConfig<SupplierItem>
    {
        /// <summary>
        /// 供应商与物料关系验证
        /// </summary>
        /// <param name="rules"> 验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    SupplierItem.SupplierIdProperty,
                    SupplierItem.ItemIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "供应商与物料关系不能重复添加".L10N();
                }
            });
            base.AddValidations(rules);
        }

        /// <summary>
        /// 链接用户与供应商关系表
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CSM_SUPPLIER_ITEM").MapAllProperties();
            Meta.Property(SupplierItem.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}