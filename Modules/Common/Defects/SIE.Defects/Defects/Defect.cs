using SIE.Defects.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects
{
    /// <summary>
    /// 缺陷代码
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DefectCriteria))]
    [Label("缺陷代码")]
    [DisplayMember(nameof(Code))]
    [QueryMembers(new[] { nameof(Code), nameof(Description) })]
    public partial class Defect : DataEntity
    {

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Defect>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("描述")]
        [MaxLength(1000)]
        public static readonly Property<string> DescriptionProperty = P<Defect>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 质量类型 QualityType
        /// <summary>
        /// 质量类型
        /// </summary>
        [Label("质量类型")]
        public static readonly Property<QualityType> QualityTypeProperty = P<Defect>.Register(e => e.QualityType);

        /// <summary>
        /// 质量类型
        /// </summary>
        public QualityType QualityType
        {
            get { return GetProperty(QualityTypeProperty); }
            set { SetProperty(QualityTypeProperty, value); }
        }
        #endregion

        #region 缺陷分类 DefectCategory
        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        [Label("缺陷分类")]
        public static readonly IRefIdProperty DefectCategoryIdProperty = P<Defect>.RegisterRefId(e => e.DefectCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        public double DefectCategoryId
        {
            get { return (double)GetRefId(DefectCategoryIdProperty); }
            set { SetRefId(DefectCategoryIdProperty, value); }
        }

        /// <summary>
        /// 缺陷分类
        /// </summary>
        public static readonly RefEntityProperty<DefectCategory> DefectCategoryProperty = P<Defect>.RegisterRef(e => e.DefectCategory, DefectCategoryIdProperty);

        /// <summary>
        /// 缺陷分类
        /// </summary>
        public DefectCategory DefectCategory
        {
            get { return GetRefEntity(DefectCategoryProperty); }
            set { SetRefEntity(DefectCategoryProperty, value); }
        }
        #endregion

        #region 缺陷等级 DefectGrade
        /// <summary>
        /// 缺陷等级Id
        /// </summary>
        [Label("缺陷等级")]
        public static readonly IRefIdProperty DefectGradeIdProperty = P<Defect>.RegisterRefId(e => e.DefectGradeId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷等级Id
        /// </summary>
        public double DefectGradeId
        {
            get { return (double)GetRefId(DefectGradeIdProperty); }
            set { SetRefId(DefectGradeIdProperty, value); }
        }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public static readonly RefEntityProperty<DefectGrade> DefectGradeProperty = P<Defect>.RegisterRef(e => e.DefectGrade, DefectGradeIdProperty);

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public DefectGrade DefectGrade
        {
            get { return GetRefEntity(DefectGradeProperty); }
            set { SetRefEntity(DefectGradeProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 缺陷等级 DefectLevel
        /// <summary>
        /// 缺陷等级
        /// </summary>
        [Label("缺陷等级")]
        public static readonly Property<string> DefectLevelProperty = P<Defect>.RegisterView(e => e.DefectLevel, p => p.DefectGrade.Name);

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string DefectLevel
        {
            get { return this.GetProperty(DefectLevelProperty); }
        }
        #endregion

        #region 严重度 DefectLevel
        /// <summary>
        /// 严重度
        /// </summary>
        [Label("严重度")]
        public static readonly Property<DefectSeverity> DefectSeverityProperty = P<Defect>.RegisterView(e => e.DefectSeverity, p => p.DefectGrade.DefectSeverity);

        /// <summary>
        /// 严重度
        /// </summary>
        public DefectSeverity DefectSeverity
        {
            get { return this.GetProperty(DefectSeverityProperty); }
        }
        #endregion

        #region 分类编码 CategoryCode
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> CategoryCodeProperty = P<Defect>.RegisterView(e => e.CategoryCode, p => p.DefectCategory.Code);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode
        {
            get { return this.GetProperty(CategoryCodeProperty); }
        }
        #endregion

        #region 描述 CategoryDescription
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> CategoryDescriptionProperty = P<Defect>.RegisterView(e => e.CategoryDescription, p => p.DefectCategory.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string CategoryDescription
        {
            get { return this.GetProperty(CategoryDescriptionProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 缺陷代码 实体配置
    /// </summary>
    internal class DefectConfig : EntityConfig<Defect>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DEF_DEFECT").MapAllProperties();
            Meta.Property(Defect.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(Defect.QualityTypeProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}