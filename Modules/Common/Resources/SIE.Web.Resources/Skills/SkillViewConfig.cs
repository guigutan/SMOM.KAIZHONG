using SIE.Domain;
using SIE.Resources.Skills;
using System.Collections.Generic;

namespace SIE.Web.Resources.Skills
{
    /// <summary>
    /// 技能清单视图配置
    /// </summary>
    public class SkillConfig : WebViewConfig<Skill>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands().UseImportCommands().UseCommands("SIE.Web.Resources.Skills.Commands.AddSkillCategoryCommand");
            View.Property(p => p.Code).ShowInList(width: 150).Readonly(f => f.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(width: 150);
            View.Property(p => p.Validity).UseSpinEditor(e => { e.MinValue = 1; e.AllowBlank = true; }).ShowInList(width: 150);
            View.Property(p => p.Category).ShowInList(width: 150).HasLabel("分类").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.CategoryName), nameof(e.Category.Name));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.Remark).ShowInList(width: 300);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.Code);
            View.Property(p => p.Validity);
            View.Property(p => p.Remark);
            View.Property(p => p.CategoryCode);
            View.Property(p => p.CategoryName);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Validity);
            View.PropertyRef(p => p.Category.Code).HasLabel("分类编码");
            View.Property(p => p.Remark);
        }
    }
}