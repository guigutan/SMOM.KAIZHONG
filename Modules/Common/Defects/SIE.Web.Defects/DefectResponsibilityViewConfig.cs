using SIE.Defects;
using System.Collections.Generic;

namespace SIE.Web.Defects
{
    /// <summary>
    /// 缺陷责任视图配置
    /// </summary>
    internal class DefectResponsibilityViewConfig : WebViewConfig<DefectResponsibility>
    {
        /// <summary>
        /// 视图通用配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();

        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseCommands("SIE.Web.Defects.Commands.AddDefectResponsibilityCategoryCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.Category).HasLabel("分类编码").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.CategoryDescription), nameof(e.Category.Description));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.CategoryDescription).HasLabel("分类描述").Readonly();
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {           
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.Category).HasLabel("分类编码");
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.CategoryCode);
            View.Property(p => p.CategoryDescription);
        }
    }
}
