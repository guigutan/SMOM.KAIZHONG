using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料与分类关系表
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料与分类关系表")]
    [DisplayMember(nameof(Id))]
    public partial class ItemCategoryRelation : DataEntity
    {
        #region 分类类型 Type
        /// <summary>
        /// 分类类型
        /// </summary>
        [Label("分类类型")]
        public static readonly Property<CategoryType> TypeProperty = P<ItemCategoryRelation>.Register(e => e.Type);

        /// <summary>
        /// 分类类型
        /// </summary>
        public CategoryType Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料ID
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemCategoryRelation>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemCategoryRelation>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 分类 ItemCategory
        /// <summary>
        /// 分类Id
        /// </summary>
        [Label("分类")]
        public static readonly IRefIdProperty ItemCategoryIdProperty = P<ItemCategoryRelation>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 分类Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)GetRefNullableId(ItemCategoryIdProperty); }
            set { SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 分类
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<ItemCategoryRelation>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return GetRefEntity(ItemCategoryProperty); }
            set { SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region RegisterView注册视图属性(关联实体属性平铺显示)

        #region 分类编码 ItemCategoryCode
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> ItemCategoryCodeProperty = P<ItemCategoryRelation>.RegisterView(e => e.ItemCategoryCode, p => p.ItemCategory.Code);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string ItemCategoryCode
        {
            get { return this.GetProperty(ItemCategoryCodeProperty); }
        }
        #endregion

        #region 分类名称 ItemCategoryName
        /// <summary>
        /// 分类名称
        /// </summary>
        [Label("分类名称")]
        public static readonly Property<string> ItemCategoryNameProperty = P<ItemCategoryRelation>.RegisterView(e => e.ItemCategoryName, p => p.ItemCategory.Name);

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ItemCategoryName
        {
            get { return this.GetProperty(ItemCategoryNameProperty); }
        }
        #endregion

        #region 分类层级ID ItemCaterotyLevelId
        /// <summary>
        /// 分类层级ID
        /// </summary>
        [Label("分类层级ID")]
        public static readonly Property<double> ItemCaterotyLevelIdProperty = P<ItemCategoryRelation>.RegisterView(e => e.ItemCaterotyLevelId, p => p.ItemCategory.LevelId);

        /// <summary>
        /// 分类层级ID
        /// </summary>
        public double ItemCaterotyLevelId
        {
            get { return this.GetProperty(ItemCaterotyLevelIdProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ItemCategoryRelation>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<ItemCategoryRelation>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 物料与分类关系表 实体配置
    /// </summary>
    internal class ItemCategoryRelationConfig : EntityConfig<ItemCategoryRelation>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_CATE_RELATION").MapAllProperties();
            Meta.Property(ItemCategoryRelation.ItemIdProperty).ColumnMeta.HasIndex();
            Meta.Property(ItemCategoryRelation.TypeProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }

        /// <summary>
        /// 增加字段的验证
        /// </summary>
        /// <param name="rules">验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.Add(ItemCategoryRelation.TypeProperty, new RequiredRule());
            rules.Add(ItemCategoryRelation.ItemIdProperty, new RequiredRule());
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    ItemCategoryRelation.TypeProperty,
                    ItemCategoryRelation.ItemIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "分类类型不能重复".L10N();
                }
            });
        }
    }

    /// <summary>
    /// 物料扩展分类列表
    /// </summary>
    [CompiledPropertyDeclarer]
    public class ItemExtCategoryListProperty
    {
        /// <summary>
        /// 物料分类扩展列表属性
        /// </summary>
        public static readonly ListProperty<EntityList<ItemCategoryRelation>> ItemCategoryListProperty =
            P<Item>.RegisterExtensionList<EntityList<ItemCategoryRelation>>("ItemCategoryList", typeof(ItemExtCategoryListProperty));

        /// <summary>
        /// 物料分类扩展列表属性
        /// </summary>
        /// <param name="me">物料对象</param>
        /// <returns>物料分类列表</returns>
        public static EntityList<ItemCategoryRelation> GetItemCategoryList(Item me)
        {
            return me.GetProperty(ItemCategoryListProperty);
        }

        /// <summary>
        /// 物料分类扩展列表属性
        /// </summary>
        /// <param name="me">物料</param>
        /// <param name="value">物料分类列表</param>
        public static void SetItemCategoryList(Item me, EntityList<ItemCategoryRelation> value)
        {
            me.SetProperty(ItemCategoryListProperty, value);
        }

        /// <summary>
        /// 物料扩展分类列表实体配置
        /// </summary>
        internal class ItemExtCategoryListPropertyConfig : EntityConfig<Item>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(ItemCategoryListProperty)?.DontMapColumn();
            }
        }
    }
}