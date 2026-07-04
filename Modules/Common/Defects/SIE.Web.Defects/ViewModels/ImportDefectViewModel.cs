using SIE.Defects;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.Defects.ViewModels
{
    /// <summary>
    /// 导入料号检验标准 实体
    /// </summary>
    [RootEntity, Serializable]
    public class ImportDefectViewModel : ViewModel
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ImportDefectViewModel>.Register(e => e.Code);

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
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<ImportDefectViewModel>.Register(e => e.Description);

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
        public static readonly Property<string> QualityTypeProperty = P<ImportDefectViewModel>.Register(e => e.QualityType);

        /// <summary>
        /// 质量类型
        /// </summary>
        public string QualityType
        {
            get { return GetProperty(QualityTypeProperty); }
            set { SetProperty(QualityTypeProperty, value); }
        }
        #endregion

        #region 缺陷分类 DefectCategory
        /// <summary>
        /// 缺陷分类
        /// </summary>
        [Label("缺陷分类")]
        public static readonly Property<string> DefectCategoryProperty = P<ImportDefectViewModel>.Register(e => e.DefectCategory);

        /// <summary>
        /// 缺陷分类
        /// </summary>
        public string DefectCategory
        {
            get { return this.GetProperty(DefectCategoryProperty); }
            set { this.SetProperty(DefectCategoryProperty, value); }
        }
        #endregion

        #region 缺陷等级 DefectGrade
        /// <summary>
        /// 缺陷等级
        /// </summary>
        [Label("缺陷等级")]
        public static readonly Property<string> DefectGradeProperty = P<ImportDefectViewModel>.Register(e => e.DefectGrade);

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string DefectGrade
        {
            get { return this.GetProperty(DefectGradeProperty); }
            set { this.SetProperty(DefectGradeProperty, value); }
        }
        #endregion

        #region ErrorMessage 导入失败原因

        /// <summary>
        /// 导入失败原因
        /// </summary>
        [Label("导入失败原因")]
        public static readonly Property<string> ErrorMessageProperty = P<ImportDefectViewModel>.Register(e => e.ErrorMessage);

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public string ErrorMessage
        {
            get { return this.GetProperty(ErrorMessageProperty); }
            set { this.SetProperty(ErrorMessageProperty, value); }
        }

        #endregion
    }

    /// <summary>
    /// 导入料号检验标准 视图配置
    /// </summary>
    class ImportExperienceCheckDataViewModelConfig : WebViewConfig<ImportDefectViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("导入失败数据");
            View.AssignAuthorize(typeof(Defect));
        }

        /// <summary>
        /// 配置列表试图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.ErrorMessage);
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.QualityType);
            View.Property(p => p.DefectCategory);
            View.Property(p => p.DefectGrade);

        }
    }
}
