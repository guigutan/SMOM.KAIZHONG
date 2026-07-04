using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkOrders._Routing_
{
    /// <summary>
    /// 单位耗用量向上取整配置表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SingleQtyRoundUpCriteria))]
    [Label("单位耗用量向上取整配置表")]
    public class SingleQtyRoundUp : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<SingleQtyRoundUp>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<SingleQtyRoundUp>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SingleQtyRoundUp>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<SingleQtyRoundUp>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 基本计量单位 Unit
        /// <summary>
        /// 基本计量单位
        /// </summary>
        [Label("基本计量单位")]
        public static readonly Property<string> UnitProperty = P<SingleQtyRoundUp>.RegisterView(e => e.Unit, p => p.Item.Unit.Code);

        /// <summary>
        /// 基本计量单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
        }
        #endregion

        #region 基本分类 ItemType
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<ItemType> ItemTypeProperty = P<SingleQtyRoundUp>.RegisterView(e => e.ItemType, p => p.Item.Type);

        /// <summary>
        /// 基本分类
        /// </summary>
        public ItemType ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
        }
        #endregion

        #region 物料类型 ItemMtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> ItemMtartProperty = P<SingleQtyRoundUp>.RegisterView(e => e.ItemMtart, p => p.Item.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string ItemMtart
        {
            get { return this.GetProperty(ItemMtartProperty); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<SingleQtyRoundUp>.RegisterView(e => e.ShortDescription, p => p.Item.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库

        #region 分类编码 ItemCategoryCode
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> ItemCategoryCodeProperty = P<SingleQtyRoundUp>.Register(e => e.ItemCategoryCode);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string ItemCategoryCode
        {
            get { return this.GetProperty(ItemCategoryCodeProperty); }
            set { this.SetProperty(ItemCategoryCodeProperty, value); }
        }
        #endregion

        #region 分类名称 ItemCategoryName
        /// <summary>
        /// 分类名称
        /// </summary>
        [Label("分类名称")]
        public static readonly Property<string> ItemCategoryNameProperty = P<SingleQtyRoundUp>.Register(e => e.ItemCategoryName);

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ItemCategoryName
        {
            get { return this.GetProperty(ItemCategoryNameProperty); }
            set { this.SetProperty(ItemCategoryNameProperty, value); }
        }
        #endregion

        #endregion
    }

    internal class SingleQtyRoundUpConfig : EntityConfig<SingleQtyRoundUp>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(SingleQtyRoundUp.ItemIdProperty, new NotDuplicateRule());
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("SINGLE_QTY_ROUND_UP").MapAllProperties();
            Meta.Property(SingleQtyRoundUp.ItemCategoryCodeProperty).DontMapColumn();
            Meta.Property(SingleQtyRoundUp.ItemCategoryNameProperty).DontMapColumn();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
