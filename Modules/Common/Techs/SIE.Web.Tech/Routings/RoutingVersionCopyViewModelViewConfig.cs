namespace SIE.Web.Tech.Routings
{
    /// <summary>
    /// 工艺路线版本复制视图配置
    /// </summary>
    internal class RoutingVersionCopyViewModelViewConfig : WebViewConfig<RoutingVersionCopyViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(1);
            View.Property(p => p.IsCopyActivityProperty).UseCheckEditor(p => p.Checked = true).ShowInDetail();
            View.Property(p => p.IsCopyBom).UseCheckEditor(p => p.Checked = true).ShowInDetail();
            View.Property(p => p.IsCopyFixture).UseCheckEditor(p => p.Checked = true).ShowInDetail();
        }
    }
}
