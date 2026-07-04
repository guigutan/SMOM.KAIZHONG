using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Items.KzItemCategorys
{
    /// <summary>
    /// 产品工艺属性维护
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("产品工艺属性维护")]
    public class KzItemCategory : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<KzItemCategory>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<KzItemCategory>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工艺属性分类 KzCategory
        /// <summary>
        /// 工艺属性分类Id
        /// </summary>
        [Label("工艺属性分类")]
        public static readonly IRefIdProperty KzCategoryIdProperty =
            P<KzItemCategory>.RegisterRefId(e => e.KzCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 工艺属性分类Id
        /// </summary>
        public double KzCategoryId
        {
            get { return (double)this.GetRefId(KzCategoryIdProperty); }
            set { this.SetRefId(KzCategoryIdProperty, value); }
        }

        /// <summary>
        /// 工艺属性分类
        /// </summary>
        public static readonly RefEntityProperty<KzCategory> KzCategoryProperty =
            P<KzItemCategory>.RegisterRef(e => e.KzCategory, KzCategoryIdProperty);

        /// <summary>
        /// 工艺属性分类
        /// </summary>
        public KzCategory KzCategory
        {
            get { return this.GetRefEntity(KzCategoryProperty); }
            set { this.SetRefEntity(KzCategoryProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<KzItemCategory>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<KzItemCategory>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 工艺属性分类编码 KzCategoryCode
        /// <summary>
        /// 工艺属性分类编码
        /// </summary>
        [Label("工艺属性分类编码")]
        public static readonly Property<string> KzCategoryCodeProperty = P<KzItemCategory>.RegisterView(e => e.KzCategoryCode, p => p.KzCategory.Code);

        /// <summary>
        /// 工艺属性分类编码
        /// </summary>
        public string KzCategoryCode
        {
            get { return this.GetProperty(KzCategoryCodeProperty); }
        }
        #endregion

        #region 工艺属性分类名称 KzCategoryName
        /// <summary>
        /// 工艺属性分类名称
        /// </summary>
        [Label("工艺属性分类名称")]
        public static readonly Property<string> KzCategoryNameProperty = P<KzItemCategory>.RegisterView(e => e.KzCategoryName, p => p.KzCategory.Name);

        /// <summary>
        /// 工艺属性分类名称
        /// </summary>
        public string KzCategoryName
        {
            get { return this.GetProperty(KzCategoryNameProperty); }
        }
        #endregion

        #endregion
    }

    internal class KzItemCategoryConfig : EntityConfig<KzItemCategory>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(KzItemCategory.ItemIdProperty, new NotDuplicateRule());
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("KZ_ITEM_CATEGORY").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
