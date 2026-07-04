using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线 
    /// </summary>
    [RootEntity, Serializable]
    [Label("工艺路线")]
    [DisplayMember(nameof(Name))]
    [CriteriaQuery]
    public partial class Routing : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Routing>.Register(e => e.Name);

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
        public static readonly Property<string> DescriptionProperty = P<Routing>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 版本列表 VersionList
        /// <summary>
        /// 版本列表
        /// </summary>
        public static readonly ListProperty<EntityList<RoutingVersion>> VersionListProperty = P<Routing>.RegisterList(e => e.VersionList);

        /// <summary>
        /// 版本列表
        /// </summary>
        public EntityList<RoutingVersion> VersionList
        {
            get { return this.GetLazyList(VersionListProperty); }
        }
        #endregion

        #region 默认版本 DefaultVersion
        /// <summary>
        /// 默认版本Id
        /// </summary>
        public static readonly IRefIdProperty DefaultVersionIdProperty = P<Routing>.RegisterRefId(e => e.DefaultVersionId, ReferenceType.Normal);

        /// <summary>
        /// 默认版本Id
        /// </summary>
        public double? DefaultVersionId
        {
            get { return (double?)GetRefNullableId(DefaultVersionIdProperty); }
            set { SetRefNullableId(DefaultVersionIdProperty, value); }
        }

        /// <summary>
        /// 默认版本
        /// </summary>
        public static readonly RefEntityProperty<RoutingVersion> DefaultVersionProperty = P<Routing>.RegisterRef(e => e.DefaultVersion, DefaultVersionIdProperty);

        /// <summary>
        /// 默认版本
        /// </summary>
        public RoutingVersion DefaultVersion
        {
            get { return GetRefEntity(DefaultVersionProperty); }
            set { SetRefEntity(DefaultVersionProperty, value); }
        }
        #endregion

        #region 产品族分类 Category
        /// <summary>
        /// 产品族分类Id
        /// </summary>
        public static readonly IRefIdProperty CategoryIdProperty = P<Routing>.RegisterRefId(e => e.CategoryId, ReferenceType.Normal);

        /// <summary>
        /// 产品族分类Id
        /// </summary>
        public double CategoryId
        {
            get { return (double)GetRefId(CategoryIdProperty); }
            set { SetRefId(CategoryIdProperty, value); }
        }

        /// <summary>
        /// 产品族分类
        /// </summary>
        public static readonly RefEntityProperty<ProductFamilyCategory> CategoryProperty = P<Routing>.RegisterRef(e => e.Category, CategoryIdProperty);

        /// <summary>
        /// 产品族分类
        /// </summary>
        public ProductFamilyCategory Category
        {
            get { return GetRefEntity(CategoryProperty); }
            set { SetRefEntity(CategoryProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工艺路线  实体配置
    /// </summary>
    internal class RoutingConfig : EntityConfig<Routing>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT").MapAllProperties();
            Meta.Property(Routing.DefaultVersionIdProperty).ColumnMeta.IgnoreFK().IsNullable();
            Meta.Property(Routing.NameProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}