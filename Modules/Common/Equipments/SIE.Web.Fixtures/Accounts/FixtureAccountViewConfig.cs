using SIE.Fixtures.Fixtures.Accounts;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
    /// 工装治具台账-视图配置
    /// </summary>
    internal class FixtureAccountViewConfig : WebViewConfig<FixtureAccount>
    {
        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).HasLabel("工治具").Readonly().HasOrderNo(1).Show(ShowInWhere.All);
            View.Property(p => p.AccountState).Readonly().HasOrderNo(3).Show(ShowInWhere.All);
            View.Property(p => p.QualityState).Readonly().HasOrderNo(5).Show(ShowInWhere.All);
            View.Property(p => p.Proprietorship).Readonly().HasOrderNo(7).Show(ShowInWhere.All);
            View.Property(p => p.ProductionDate).Readonly().HasOrderNo(9).Show(ShowInWhere.All);
        }
    }
}