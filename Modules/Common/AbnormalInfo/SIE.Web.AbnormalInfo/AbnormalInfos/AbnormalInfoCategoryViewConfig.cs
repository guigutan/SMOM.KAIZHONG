using SIE.AbnormalInfo.AbnormalInfos;

namespace SIE.Web.AbnormalInfo.AbnormalInfoss
{
    /// <summary>
    /// 异常信息分类视图配置
    /// </summary>
    public class AbnormalInfoCategoryViewConfig: WebViewConfig<AbnormalInfoCategory>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AbnormalInfoDefinition));
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).ShowInList(width: 160);
            View.Property(p => p.Desc).ShowInList(width: 300);
            View.ChildrenProperty(p => p.SendUpgradeSetList);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).HasLabel("编码");
            View.Property(p => p.Desc).HasLabel("描述");
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Desc);
        }
    }
}
