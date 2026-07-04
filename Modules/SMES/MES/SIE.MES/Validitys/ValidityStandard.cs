using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Validitys
{
    /// <summary>
    /// 有效期标准维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ValidityStandardCriteria))]
    [Label("有效期标准维护")]
    public class ValidityStandard : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ValidityStandard>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<ValidityStandard>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<ValidityStandard>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性值 ItemExtPropName
        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        [Label("物料扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<ValidityStandard>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 可用时长寿命(H) LongLived
        /// <summary>
        /// 可用时长寿命(H)
        /// </summary>
        [Label("可用时长寿命")]
        public static readonly Property<decimal> LongLivedProperty = P<ValidityStandard>.Register(e => e.LongLived);

        /// <summary>
        /// 可用时长寿命(H)
        /// </summary>
        public decimal LongLived
        {
            get { return this.GetProperty(LongLivedProperty); }
            set { this.SetProperty(LongLivedProperty, value); }
        }
        #endregion

        #region 生效日期 Effective
        /// <summary>
        /// 生效日期
        /// </summary>
        [Label("生效日期")]
        public static readonly Property<DateTime> EffectiveProperty = P<ValidityStandard>.Register(e => e.Effective);

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime Effective
        {
            get { return this.GetProperty(EffectiveProperty); }
            set { this.SetProperty(EffectiveProperty, value); }
        }
        #endregion

        #region 失效日期 Expiration
        /// <summary>
        /// 失效日期
        /// </summary>
        [Label("失效日期")]
        public static readonly Property<DateTime?> ExpirationProperty = P<ValidityStandard>.Register(e => e.Expiration);

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? Expiration
        {
            get { return this.GetProperty(ExpirationProperty); }
            set { this.SetProperty(ExpirationProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ValidityStandard>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType?> ItemTypeProperty = P<ValidityStandard>.RegisterView(e => e.ItemType, p => p.Item.Type);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType? ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
        }
        #endregion

        #region 是否启用物料扩展属性 EnableExtProp
        /// <summary>
        /// 是否启用物料扩展属性
        /// </summary>
        [Label("是否启用物料扩展属性")]
        public static readonly Property<bool> EnableExtPropProperty = P<ValidityStandard>.RegisterView(e => e.EnableExtProp, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 是否启用物料扩展属性
        /// </summary>
        public bool EnableExtProp
        {
            get { return this.GetProperty(EnableExtPropProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 数据源配置
    /// </summary>
    public class ValidityStandardConfig : EntityConfig<ValidityStandard>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_VALI_STAND").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
