using SIE.Core.Labels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.ProductIntfc.InspSettings;
using System;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库参数
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductStorageParamCriteria))]
    [Label("成品入库参数")]
    public partial class ProductStorageParam : DataEntity
    {
        #region 入库数量 Qty
        /// <summary>
        /// 入库数量
        /// </summary>
        [Label("入库数量")]
        public static readonly Property<decimal> QtyProperty = P<ProductStorageParam>.Register(e => e.Qty);

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 产品 Item
        /// <summary>
        /// 产品Id
        /// </summary> 
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<ProductStorageParam>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ProductStorageParam>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 报检维度 InspDimension
        /// <summary>
        /// 报检维度
        /// </summary>
        [Label("报检维度")]
        public static readonly Property<InspDimension> InspDimensionProperty = P<ProductStorageParam>.Register(e => e.InspDimension);

        /// <summary>
        /// 报检维度
        /// </summary>
        public InspDimension InspDimension
        {
            get { return GetProperty(InspDimensionProperty); }
            set { SetProperty(InspDimensionProperty, value); }
        }
        #endregion

        #region BS

        #region 产品编码 ItemCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<ProductStorageParam>.RegisterView(e => e.ItemCode, e => e.Item.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<ProductStorageParam>.RegisterView(e => e.ItemName, e => e.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 视图属性
        #region 产品类型 ItemType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Label("产品类型")]
        public static readonly Property<ItemType> ItemTypeProperty = P<ProductStorageParam>.RegisterView(e => e.ItemType, e => e.Item.Type);

        /// <summary>
        /// 产品类型
        /// </summary>
        public ItemType ItemType
        {
            get { return GetProperty(ItemTypeProperty); }
        }
        #endregion

        #endregion
        #endregion
    }

        /// <summary>
        /// 成品入库参数 实体配置
        /// </summary>
        internal class ProductStorageParamConfig : EntityConfig<ProductStorageParam>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules"></param>

        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var entity = o as ProductStorageParam;
                    if (entity.Qty < 1)
                    {
                        e.BrokenDescription = "入库参数最小值1，当前值{0}！".L10nFormat(entity.Qty);
                    }
                }
            });
        }

        /// <summary>
        /// 数据表Config
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_PRO_STO_PARAM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}