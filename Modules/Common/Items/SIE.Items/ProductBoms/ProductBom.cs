using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.ProjectMaintains;
using SIE.Domain;
using SIE.Items.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
	/// 产品BOM
	/// </summary>
	[RootEntity, Serializable]
    [EntityWithConfig(typeof(ProductBomVersionConfig))]
    [DisplayMember(nameof(Code))]
    [ConditionQueryType(typeof(ProductBomCriteria))]
    [Label("产品BOM")]
    public partial class ProductBom : DataEntity
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ProductBom()
        {
            UpdateDate = DateTime.Now;
        }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("BOM编码")]
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        public static readonly Property<string> CodeProperty = P<ProductBom>.Register(e => e.Code);

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
        [Label("BOM名称")]
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        public static readonly Property<string> NameProperty = P<ProductBom>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 版本 Version
        /// <summary>
        /// 版本
        /// </summary>
        [Label("版本号")]
        public static readonly Property<string> VersionProperty = P<ProductBom>.Register(e => e.Version);

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get { return GetProperty(VersionProperty); }
            set { SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认
        /// </summary>
        [Label("是否默认")]
        public static readonly Property<bool> IsDefaultProperty = P<ProductBom>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 数据来源类型 SourceType
        /// <summary>
        /// 数据来源类型
        /// </summary>
        [Label("数据来源类型")]
        public static readonly Property<SourceType> SourceTypeProperty = P<ProductBom>.Register(e => e.SourceType);

        /// <summary>
        /// 数据来源类型
        /// </summary>
        public SourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<ProductBom>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<ProductBom>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProductBom>.RegisterView(e => e.ProductCode, p => p.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProductBom>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<ProductBom>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtPropName
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<ProductBom>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 规格型号 ProductSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ProductSpecificationModelProperty = P<ProductBom>.RegisterView(e => e.ProductSpecificationModel, p => p.Product.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ProductSpecificationModel
        {
            get { return this.GetProperty(ProductSpecificationModelProperty); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<ProductBom>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 单位 ProductUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ProductUnitNameProperty = P<ProductBom>.RegisterView(e => e.ProductUnitName, p => p.Product.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ProductUnitName
        {
            get { return this.GetProperty(ProductUnitNameProperty); }
        }
        #endregion

        #region 是否启用扩展属性 EnableExtProp
        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        [Label("是否启用扩展属性")]
        public static readonly Property<string> EnableExtPropProperty = P<ProductBom>.RegisterView(e => e.EnableExtProp, p => p.Product.EnableExtendProperty);

        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        public string EnableExtProp
        {
            get { return this.GetProperty(EnableExtPropProperty); }
        }
        #endregion

        #region 产品与BOM关系 DetailList
        /// <summary>
        /// 产品与BOM关系
        /// </summary>
        [Label("产品与BOM关系")]
        public static readonly ListProperty<EntityList<ProductBomDetail>> DetailListProperty = P<ProductBom>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 产品与BOM关系
        /// </summary>
        public EntityList<ProductBomDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 产品BOM与属性值关系 PropertyValueList
        /// <summary>
        /// 产品BOM与属性值关系
        /// </summary>
        [Label("产品BOM与属性值关系")]
        public static readonly ListProperty<EntityList<ProductBomPropertyValue>> PropertyValueListProperty = P<ProductBom>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 产品BOM与属性值关系
        /// </summary>
        public EntityList<ProductBomPropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 组合替代列表 CombinationReplateList
        /// <summary>
        /// 组合替代列表
        /// </summary>
        [Label("组合替代")]
        public static readonly ListProperty<EntityList<CombinationReplate>> CombinationReplateListProperty = P<ProductBom>.RegisterList(e => e.CombinationReplateList);
        /// <summary>
        /// 组合替代列表
        /// </summary>
        public EntityList<CombinationReplate> CombinationReplateList
        {
            get { return this.GetLazyList(CombinationReplateListProperty); }
        }
        #endregion

        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<ProductBom>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectMaintainId
        {
            get { return (double?)this.GetRefNullableId(ProjectMaintainIdProperty); }
            set { this.SetRefNullableId(ProjectMaintainIdProperty, value); }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        public static readonly RefEntityProperty<ProjectMaintain> ProjectMaintainProperty =
            P<ProductBom>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

        /// <summary>
        /// 项目号
        /// </summary>
        public ProjectMaintain ProjectMaintain
        {
            get { return this.GetRefEntity(ProjectMaintainProperty); }
            set { this.SetRefEntity(ProjectMaintainProperty, value); }
        }
        #endregion

        #region 项目号编码 ProjectMaintainCode
        /// <summary>
        /// 项目号编码
        /// </summary>
        [Label("项目号编码")]
        public static readonly Property<string> ProjectMaintainCodeProperty = P<ProductBom>.RegisterView(e => e.ProjectMaintainCode, p => p.ProjectMaintain.Code);

        /// <summary>
        /// 项目号编码
        /// </summary>
        public string ProjectMaintainCode
        {
            get { return this.GetProperty(ProjectMaintainCodeProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 产品BOM 实体配置
    /// </summary>
    internal class ProductBomConfig : EntityConfig<ProductBom>
    {
        /// <summary>
        /// 对 Meta 属性的配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROD_BOM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}