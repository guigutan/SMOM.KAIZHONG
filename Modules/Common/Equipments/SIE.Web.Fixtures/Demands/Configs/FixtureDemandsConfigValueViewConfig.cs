using SIE.Fixtures.FixtureDemands.Config;

namespace SIE.Web.Fixtures.Demands.Configs
{
    /// <summary>
    /// p配置项
    /// </summary>
    public  class FixtureDemandsConfigValueViewConfig : WebViewConfig<FixtureDemandsConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.SwitchApproval).Show();
            View.Property(p => p.ApprovalWay).Show().Visibility(p => p.SwitchApproval);
        }
    }
}
