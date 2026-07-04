using SIE.Domain;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Reports
{
    /// <summary>
    /// 异常信息报表 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("异常信息报表查询实体")]
    public class AbnormalInfoReportCriteria : Criteria
    {
        #region 异常分类 AbnormalCategory
        /// <summary>
        /// 异常分类Id
        /// </summary>
        [Label("异常分类")]
        public static readonly IRefIdProperty AbnormalCategoryIdProperty =
            P<AbnormalInfoReportCriteria>.RegisterRefId(e => e.AbnormalCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 异常分类Id
        /// </summary>
        public double? AbnormalCategoryId
        {
            get { return (double?)this.GetRefNullableId(AbnormalCategoryIdProperty); }
            set { this.SetRefNullableId(AbnormalCategoryIdProperty, value); }
        }

        /// <summary>
        /// 异常分类
        /// </summary>
        public static readonly RefEntityProperty<AbnormalInfoCategory> AbnormalCategoryProperty =
            P<AbnormalInfoReportCriteria>.RegisterRef(e => e.AbnormalCategory, AbnormalCategoryIdProperty);

        /// <summary>
        /// 异常分类
        /// </summary>
        public AbnormalInfoCategory AbnormalCategory
        {
            get { return this.GetRefEntity(AbnormalCategoryProperty); }
            set { this.SetRefEntity(AbnormalCategoryProperty, value); }
        }
        #endregion

        #region 异常信息定义 AbnormalDefinition
        /// <summary>
        /// 异常信息定义Id
        /// </summary>
        [Label("异常信息")]
        public static readonly IRefIdProperty AbnormalDefinitionIdProperty =
            P<AbnormalInfoReportCriteria>.RegisterRefId(e => e.AbnormalDefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 异常信息定义Id
        /// </summary>
        public double? AbnormalDefinitionId
        {
            get { return (double?)this.GetRefNullableId(AbnormalDefinitionIdProperty); }
            set { this.SetRefNullableId(AbnormalDefinitionIdProperty, value); }
        }

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public static readonly RefEntityProperty<AbnormalInfoDefinition> AbnormalDefinitionProperty =
            P<AbnormalInfoReportCriteria>.RegisterRef(e => e.AbnormalDefinition, AbnormalDefinitionIdProperty);

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public AbnormalInfoDefinition AbnormalDefinition
        {
            get { return this.GetRefEntity(AbnormalDefinitionProperty); }
            set { this.SetRefEntity(AbnormalDefinitionProperty, value); }
        }
        #endregion

        #region 异常发生时间 CreateDate
        /// <summary>
        /// 异常发生时间
        /// </summary>
        [Label("异常发生时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<AbnormalInfoReportCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 异常发生时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion


        #region 视图属性
        #region 异常分类名称 AbnormalCategortyName
        /// <summary>
        /// 异常分类名称
        /// </summary>
        [Label("异常分类名称")]
        public static readonly Property<string> AbnormalCategortyNameProperty = P<AbnormalInfoReportCriteria>.RegisterView(e => e.AbnormalCategortyName, p => p.AbnormalCategory.Desc);

        /// <summary>
        /// 异常分类名称
        /// </summary>
        public string AbnormalCategortyName
        {
            get { return this.GetProperty(AbnormalCategortyNameProperty); }
        }
        #endregion

        #region 异常信息定义名称 AbnormalDefinitionDesc
        /// <summary>
        /// 异常信息定义名称
        /// </summary>
        [Label("异常信息定义名称")]
        public static readonly Property<string> AbnormalDefinitionDescProperty = P<AbnormalInfoReportCriteria>.RegisterView(e => e.AbnormalDefinitionDesc, p => p.AbnormalDefinition.Desc);

        /// <summary>
        /// 异常信息定义名称
        /// </summary>
        public string AbnormalDefinitionDesc
        {
            get { return this.GetProperty(AbnormalDefinitionDescProperty); }
        }
        #endregion

        #endregion
    }
}
