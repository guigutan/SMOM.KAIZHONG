using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
    /// 库存台帐查询实体视图配置
    /// </summary>
    internal class FixtureAccountStockCriteriaViewConfig : WebViewConfig<FixtureAccountStockCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseCode).Show(ShowInWhere.Detail);
                View.Property(p => p.WarehouseName).Show(ShowInWhere.Detail);
                View.Property(p => p.LocationCode).Show(ShowInWhere.Detail);
                View.Property(p => p.LocationName).Show(ShowInWhere.Detail);
                View.Property(p => p.AccountCode).Show(ShowInWhere.Detail);
                View.Property(p => p.EncodeCode).Show(ShowInWhere.Detail);
                View.Property(p => p.ModelCode).Show(ShowInWhere.Detail);
                View.Property(p => p.ModelName).Show(ShowInWhere.Detail);
                View.Property(p => p.FixtureType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypes(pagingInfo, keyword);
                }).Show(ShowInWhere.Detail);
            }
        }
    }
}
