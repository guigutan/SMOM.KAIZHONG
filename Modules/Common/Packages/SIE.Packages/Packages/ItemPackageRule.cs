
using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages
{
    /// <summary>
    /// 物料包装规则
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("物料包装规则")]
    [DisplayMember(nameof(Name))]
    public partial class ItemPackageRule : DataEntity
    {
        #region 包装规则 PackageRule
        /// <summary>
        /// 包装规则Id(仅仅作为基础数据选择的时候使用，其他业务不要使用)
        /// </summary>
        [Label("包装规则")]
        public static readonly IRefIdProperty PackageRuleIdProperty = P<ItemPackageRule>.RegisterRefId(e => e.PackageRuleId, ReferenceType.Normal);

        /// <summary>
        /// 包装规则Id(仅仅作为基础数据选择的时候使用，其他业务不要使用)
        /// </summary>
        public double PackageRuleId
        {
            get { return (double)GetRefId(PackageRuleIdProperty); }
            set { SetRefId(PackageRuleIdProperty, value); }
        }

        /// <summary>
        /// 包装规则(仅仅作为基础数据选择的时候使用，其他业务不要使用)
        /// </summary>
        public static readonly RefEntityProperty<PackageRule> PackageRuleProperty = P<ItemPackageRule>.RegisterRef(e => e.PackageRule, PackageRuleIdProperty);

        /// <summary>
        /// 包装规则(仅仅作为基础数据选择的时候使用，其他业务不要使用)
        /// </summary>
        public PackageRule PackageRule
        {
            get { return GetRefEntity(PackageRuleProperty); }
            set { SetRefEntity(PackageRuleProperty, value); }
        }
        #endregion

        #region 名称 PackageRuleName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> PackageRuleNameProperty = P<ItemPackageRule>.RegisterView(e => e.PackageRuleName, p => p.PackageRule.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string PackageRuleName
        {
            get { return this.GetProperty(PackageRuleNameProperty); }
        }
        #endregion

        #region 编码 PackageRuleCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> PackageRuleCodeProperty = P<ItemPackageRule>.RegisterView(e => e.PackageRuleCode, p => p.PackageRule.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string PackageRuleCode
        {
            get { return this.GetProperty(PackageRuleCodeProperty); }
        }
        #endregion

        #region 描述 PackageRuleDescription
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> PackageRuleDescriptionProperty = P<ItemPackageRule>.RegisterView(e => e.PackageRuleDescription, p => p.PackageRule.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string PackageRuleDescription
        {
            get { return this.GetProperty(PackageRuleDescriptionProperty); }
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        public static readonly Property<string> CodeProperty = P<ItemPackageRule>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [Required]
        public static readonly Property<string> NameProperty = P<ItemPackageRule>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<ItemPackageRule>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认
        /// </summary>
        [Label("是否默认")]
        public static readonly Property<bool> IsDefaultProperty = P<ItemPackageRule>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion  

        #region 物料与包装规则关系 Item
        /// <summary>
        /// 物料与包装规则关系Id
        /// </summary>
        [Label("物料与包装规则关系")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemPackageRule>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

        /// <summary>
        /// 物料与包装规则关系Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料与包装规则关系
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemPackageRule>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料与包装规则关系
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料包装规则明细列表 ItemPackageRuleDetailList
        /// <summary>
        /// 物料包装规则明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<ItemPackageRuleDetail>> ItemPackageRuleDetailListProperty = P<ItemPackageRule>.RegisterList(e => e.ItemPackageRuleDetailList);

        /// <summary>
        /// 物料包装规则明细列表
        /// </summary>
        public EntityList<ItemPackageRuleDetail> ItemPackageRuleDetailList
        {
            get { return this.GetLazyList(ItemPackageRuleDetailListProperty); }
        }
        #endregion

        #region 注册视图
        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ItemPackageRule>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion


        #region 物料是否扩展 ItemEnableExtendProperty
        /// <summary>
        /// 物料是否扩展
        /// </summary>
        public static readonly Property<bool> ItemEnableExtendPropertyProperty = P<ItemPackageRule>
            .RegisterView(e => e.ItemEnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 物料是否扩展
        /// </summary>
        public bool ItemEnableExtendProperty
        {
            get { return this.GetProperty(ItemEnableExtendPropertyProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 物料包装规则 实体配置
    /// </summary>
    internal class ItemPackageRuleConfig : EntityConfig<ItemPackageRule>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_IN_PKG_RULE").MapAllProperties();
            Meta.Property(ItemPackageRule.ItemIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 物料包装规则扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class ItemPackageRuleDetailProperty
    {
        /// <summary>
        /// 物料包装设置属性
        /// </summary>
        public static readonly ListProperty<EntityList<ItemPackageRule>> ItemPackageRuleListProperty =
            P<Item>.RegisterExtensionList<EntityList<ItemPackageRule>>("ItemPackageRuleList", typeof(ItemPackageRuleDetailProperty));

        /// <summary>
        /// 获取物料包装对象
        /// </summary>
        /// <param name="me">物料对象</param>
        /// <returns>返回物料包装对象</returns>
        public static EntityList<ItemPackageRule> GetItemPackageRuleList(Item me)
        {
            return me.GetProperty(ItemPackageRuleListProperty);
        }

        /// <summary>
        /// 设置物料包装对象
        /// </summary>
        /// <param name="me">物料</param>
        /// <param name="value">需要设置的物料包装对象</param>
        public static void SetItemPackageRuleList(Item me, EntityList<ItemPackageRule> value)
        {
            me.SetProperty(ItemPackageRuleListProperty, value);
        }

        /// <summary>
        /// 物料包装规则 实体配置
        /// </summary>
        internal class ItemPackageRuleDetailPropertyConfig : EntityConfig<Item>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(ItemPackageRuleDetailProperty.ItemPackageRuleListProperty)?.DontMapColumn();
            }
        }
    }
}