using SIE.Domain;
using SIE.MES.TeamManagement.RatedItems;

namespace SIE.Web.MES.TeamManagement.RatedItems
{
    /// <summary>
    /// 评分项目分类视图配置
    /// </summary>
    public class RatedItemCategoryViewConfig : WebViewConfig<RatedItemCategory>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(RatedItem));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands().UseImportCommands();
            View.Property(p => p.Code).ShowInList(width: 150).Readonly(f => f.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(width: 150);
            View.Property(p => p.Remark).ShowInList(width: 300);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 下拉视图配
        /// </summary>
        protected override void ConfigSelectionView()
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
            View.Property(p => p.Remark);
        }
    }
}