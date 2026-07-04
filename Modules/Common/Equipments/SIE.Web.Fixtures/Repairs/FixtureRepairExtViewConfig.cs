using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Repairs;

namespace SIE.Web.Fixtures.Repairs
{
    /// <summary>
    /// ID类工治具台帐扩展视图
    /// </summary>
    public class IDAccountRepairExtViewConfig : WebViewConfig<FixtureIDAccount>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssociateChildrenProperty(IDAccountRepairProperty.RepairDetailListProperty, (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var account = args.Parent as FixtureAccount;
                var repairDetails = RT.Service.Resolve<ElecFixtureController>().GetFixtureRepairDetailsByAccountId(account.Id, args.PagingInfo);
                return repairDetails;
            }, FixtureRepairDetailViewConfig.AccountRepairView).HasLabel("维修履历").OrderNo = 45;
        }
    }

    /// <summary>
    /// 编码类工治具台帐扩展视图
    /// </summary>
    public class CodeAccountRepairExtViewConfig : WebViewConfig<FixtureCodeAccount>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssociateChildrenProperty(CodeAccountRepairProperty.RepairDetailListProperty, (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var account = args.Parent as FixtureAccount;
                var repairDetails = RT.Service.Resolve<ElecFixtureController>().GetFixtureRepairDetailsByAccountId(account.Id, args.PagingInfo);
                return repairDetails;
            }, FixtureRepairDetailViewConfig.AccountRepairView).HasLabel("维修履历").OrderNo = 45;
        }
    }
}
