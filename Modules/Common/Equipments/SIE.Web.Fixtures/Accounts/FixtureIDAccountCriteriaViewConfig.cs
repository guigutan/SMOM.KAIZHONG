using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
    /// 工治具台账（ID管理）查询实体-界面
    /// </summary>
    internal class FixtureIDAccountCriteriaViewConfig : WebViewConfig<FixtureIDAccountCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureEncodeCode).Show(ShowInWhere.Detail);
                View.Property(p => p.ModelCode).Show(ShowInWhere.Detail);
                View.Property(p => p.ModelName).Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
                }).Show(ShowInWhere.Detail);
                View.Property(p => p.AccountState).Show(ShowInWhere.Detail);
                View.Property(p => p.QualityState).Show(ShowInWhere.Detail);
                View.Property(p => p.IsExceed).Show(ShowInWhere.Detail);
            }
        }
    }
}
