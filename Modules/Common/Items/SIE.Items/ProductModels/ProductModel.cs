using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品机型
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductModelCriteria))]
    [Label("产品机型")]
    [DisplayMember(nameof(Name))]
    public partial class ProductModel : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ProductModel>.Register(e => e.Code);

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
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ProductModel>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 工时(单位/小时) WorkingHours
        /// <summary>
        /// 工时(单位/小时)
        /// </summary>
        [Label("工时（单位/小时）")]
        [Required]
        [MaxValue(36000)]
        [MinValue(0)]
        public static readonly Property<decimal?> WorkingHoursProperty = P<ProductModel>.Register(e => e.WorkingHours);

        /// <summary>
        /// 工时(单位/小时)
        /// </summary>
        public decimal? WorkingHours
        {
            get { return GetProperty(WorkingHoursProperty); }
            set { SetProperty(WorkingHoursProperty, value); }
        }
        #endregion

        #region 产品族
        /// <summary>
        /// 产品族id
        /// </summary>
        public static readonly IRefIdProperty ProductFamilyIdProperty =
            P<ProductModel>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族id
        /// </summary>
        public double? ProductFamilyId
        {
            get { return (double?)this.GetRefNullableId(ProductFamilyIdProperty); }
            set { this.SetRefNullableId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族
        /// </summary>
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty =
            P<ProductModel>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return this.GetRefEntity(ProductFamilyProperty); }
            set { this.SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        #region 配送周期(单位/小时) SendingHours
        /// <summary>
        /// 配送周期(单位/小时)
        /// </summary>
        [Label("配送周期（小时）")]
        [Required]
        [MaxValue(36000)]
        [MinValue(0)]
        public static readonly Property<decimal?> SendingHoursProperty = P<ProductModel>.Register(e => e.SendingHours);

        /// <summary>
        /// 配送周期(单位/小时)
        /// </summary>
        public decimal? SendingHours
        {
            get { return GetProperty(SendingHoursProperty); }
            set { SetProperty(SendingHoursProperty, value); }
        }
        #endregion

        #region 机型技能 SkillList
        /// <summary>
        /// 机型技能
        /// </summary>
        [Label("机型技能")]
        public static readonly ListProperty<EntityList<ProductModelSkill>> SkillListProperty = P<ProductModel>.RegisterList(e => e.SkillList);

        /// <summary>
        /// 机型技能
        /// </summary>
        public EntityList<ProductModelSkill> SkillList
        {
            get { return this.GetLazyList(SkillListProperty); }
        }
        #endregion

        #region 产品族编码 ProductFamilyCode
        /// <summary>
        /// 产品族编码
        /// </summary>
        [Label("产品族编码")]
        public static readonly Property<string> ProductFamilyCodeProperty = P<ProductModel>.RegisterView(e => e.ProductFamilyCode, p => p.ProductFamily.Code);

        /// <summary>
        /// 产品族编码
        /// </summary>
        public string ProductFamilyCode
        {
            get { return this.GetProperty(ProductFamilyCodeProperty); }
        }
        #endregion

        #region 产品族名称 QualityCategoryName
        /// <summary>
        /// 产品族名称
        /// </summary>
        [Label("产品族名称")]
        public static readonly Property<string> ProductFamilyNameProperty = P<ProductModel>.RegisterView(e => e.ProductFamilyName, p => p.ProductFamily.Name);

        /// <summary>
        /// 产品族名称
        /// </summary>
        public string ProductFamilyName
        {
            get { return this.GetProperty(ProductFamilyNameProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 产品机型 实体配置
    /// </summary>
    internal class ProductModelConfig : EntityConfig<ProductModel>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRODUCT_MODEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}