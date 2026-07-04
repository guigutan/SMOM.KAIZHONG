using SIE.Fixtures.Fixtures.Accounts;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
    /// 工治具台帐-feeder详情界面
    /// </summary>
    internal class FixtureAccountToolViewConfig : WebViewConfig<FixtureAccountTool>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(FixtureAccountModel));
            View.Property(p => p.LabelCode);
            View.Property(p => p.UseNum);
            View.Property(p => p.MaintainedUseNum);
            View.Property(p => p.TotalThrowQty);
            View.Property(p => p.MaintainedThrowQty);
        }
    }
}
