using SIE.Domain;
using SIE.MES.TeamManagement.RatedItems;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.MES.TeamManagement.RatedItems
{
    /// <summary>
    /// 评分项目视图配置
    /// </summary>
    public class RatedItemConfig : WebViewConfig<RatedItem>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands().UseImportCommands().UseCommands("SIE.Web.MES.TeamManagement.RatedItems.Commands.RatedItemCategoryCommand");
            View.UseCommands("SIE.Web.MES.TeamManagement.RatedItems.Commands.RatedItemSystemCommand");
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.MES.TeamManagement.RatedItems.Commands.RatedItemEditCommand");
            View.RemoveCommands(WebCommandNames.Copy);
            View.Property(p => p.Code).ShowInList(width: 150).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(width: 150).Readonly(p => p.IsSystem)
                .UseListSetting(e => { e.HelpInfo = "系统评分项目不可编辑"; });
            View.Property(p => p.Category).ShowInList(width: 150).HasLabel("分类").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.CategoryName), nameof(e.Category.Name));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.MinScore).ShowInList(width: 150);
            View.Property(p => p.MaxScore).ShowInList(width: 150);
            View.Property(p => p.State).ShowInList(width: 150);
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
            // 下拉视图配置
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
            View.PropertyRef(p => p.Category.Code).HasLabel("评分项目分类编码");
            View.Property(p => p.MinScore);
            View.Property(p => p.MaxScore);
            View.Property(p => p.State);
        }
    }
}