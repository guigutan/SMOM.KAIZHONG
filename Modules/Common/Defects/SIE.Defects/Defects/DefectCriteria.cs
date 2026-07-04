using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Defects.Defects
{
    /// <summary>
    /// 缺陷代码查询条件
    /// </summary>
    [QueryEntity, Serializable]
    [Label("缺陷代码查询条件")]
    public class DefectCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(20)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<DefectCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> DescriptionProperty = P<DefectCriteria>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 缺陷分类 DefectCategory
        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        [Label("缺陷分类")]
        public static readonly IRefIdProperty DefectCategoryIdProperty = P<DefectCriteria>.RegisterRefId(e => e.DefectCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        public double? DefectCategoryId
        {
            get { return (double?)GetRefNullableId(DefectCategoryIdProperty); }
            set { SetRefNullableId(DefectCategoryIdProperty, value); }
        }

        /// <summary>
        /// 缺陷分类
        /// </summary>
        public static readonly RefEntityProperty<DefectCategory> DefectCategoryProperty = P<DefectCriteria>.RegisterRef(e => e.DefectCategory, DefectCategoryIdProperty);

        /// <summary>
        /// 缺陷分类
        /// </summary>
        public DefectCategory DefectCategory
        {
            get { return GetRefEntity(DefectCategoryProperty); }
            set { SetRefEntity(DefectCategoryProperty, value); }
        }
        #endregion

        #region 质量类型 QualityType
        /// <summary>
        /// 质量类型
        /// </summary>
        [Label("质量类型")]
        public static readonly Property<QualityType?> QualityTypeProperty = P<DefectCriteria>.Register(e => e.QualityType);

        /// <summary>
        /// 质量类型
        /// </summary>
        public QualityType? QualityType
        {
            get { return GetProperty(QualityTypeProperty); }
            set { SetProperty(QualityTypeProperty, value); }
        }
        #endregion

        #region 缺陷等级 DefectGrade
        /// <summary>
        /// 缺陷等级Id
        /// </summary>
        [Label("缺陷等级")]
        public static readonly IRefIdProperty DefectGradeIdProperty = P<DefectCriteria>.RegisterRefId(e => e.DefectGradeId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷等级Id
        /// </summary>
        public double? DefectGradeId
        {
            get { return (double?)GetRefNullableId(DefectGradeIdProperty); }
            set { SetRefNullableId(DefectGradeIdProperty, value); }
        }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public static readonly RefEntityProperty<DefectGrade> DefectGradeProperty = P<DefectCriteria>.RegisterRef(e => e.DefectGrade, DefectGradeIdProperty);

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public DefectGrade DefectGrade
        {
            get { return GetRefEntity(DefectGradeProperty); }
            set { SetRefEntity(DefectGradeProperty, value); }
        }
        #endregion

        #region 过滤掉ID FilterId
        /// <summary>
        /// 过滤属性值ID
        /// </summary>
        [Label("过滤掉ID")]
        public static readonly Property<double[]> FilterIdProperty = P<DefectCriteria>.Register(e => e.FilterId);

        /// <summary>
        /// 过滤属性值ID
        /// </summary>
        public double[] FilterId
        {
            get { return this.GetProperty(FilterIdProperty); }
            set { this.SetProperty(FilterIdProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询实体默认查询方法
        /// </summary>
        /// <returns>EntityList</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DefectController>().GetDefects(this);
        }
    }
}
