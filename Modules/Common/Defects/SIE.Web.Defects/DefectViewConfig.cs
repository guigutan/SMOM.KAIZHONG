using SIE.Defects;
using SIE.Web.Common;
using SIE.Web.Defects.Commands;
using System.Collections.Generic;

namespace SIE.Web.Defects
{
    /// <summary>
    /// 缺陷代码视图配置
    /// </summary>
    public class DefectViewConfig : WebViewConfig<Defect>
    {
        /// <summary>
        /// 缺陷代码选择
        /// </summary>
        public static readonly string SelectionModelView = "SelectionModelView";
        private const string DEFECT_TYPE = "缺陷等级非快码类型";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.DeclareExtendViewGroup(SelectionModelView);
            if (ViewGroup == SelectionModelView)
                DefectSelectionView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {

            View.UseDefaultCommands().UseCommands("SIE.Web.Defects.Commands.AddDefectCategoryCommand");
            View.UseCommands(typeof(ImportDefectCommand).FullName);
            View.UseClientOrder();
            View.Property(p => p.Code).ShowInList(width: 150);
            View.Property(p => p.Description);
            View.Property(p => p.DefectGradeId).UseListSetting(e => { e.HelpInfo = DEFECT_TYPE; });
            View.Property(p => p.QualityType);
            View.Property(p => p.DefectCategoryId).HasLabel("缺陷分类编码").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.CategoryDescription), nameof(e.DefectCategory.Description));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.CategoryDescription).HasLabel("缺陷分类描述").Readonly();
        }

        /// <summary>
        /// 查询面板视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.DefectCategory);
            View.Property(p => p.QualityType);
            View.Property(p => p.DefectGrade) .UseListSetting(e => { e.HelpInfo = DEFECT_TYPE; });
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.DefectGrade).UseListSetting(e => { e.HelpInfo = DEFECT_TYPE; });
            View.Property(p => p.QualityType);
            View.Property(p => p.CategoryCode);
            View.Property(p => p.CategoryDescription);
        }
        /// <summary>
        /// 报工选择视图配置
        /// </summary>
        private void DefectSelectionView()
        {
            View.UseGridSelectionModel();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Description).Show(ShowInWhere.All);
                View.Property(p => p.DefectGrade).UseListSetting(e => { e.HelpInfo = DEFECT_TYPE; });
                View.Property(p => p.QualityType).Show(ShowInWhere.All);
                View.Property(p => p.CategoryCode).Show(ShowInWhere.All);
                View.Property(p => p.CategoryDescription).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}
