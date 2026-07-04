using SIE.Fixtures.Fixtures.Accounts;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
	/// 库存详情视图配置
	/// </summary>
	public class FixtureAccountStockViewConfig : WebViewConfig<FixtureAccountStock>
    {
        /// <summary>
        /// 自定义台帐的库存详情视图
        /// </summary>
        public const string AccountListView = "AccountListView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AccountListView);
            if (ViewGroup == AccountListView)
            {
                ConfigAccountListView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {            
            //View.UseDefaultCommands();
            View.Property(p => p.AccountCode);
            View.Property(p => p.EncodeCode);
            View.Property(p => p.ModelCode);
            View.Property(p => p.ModelName);
            View.Property(p => p.FixtureTypeCode);
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.WarehouseName);
            View.Property(p => p.LocationCode);
            View.Property(p => p.LocationName);
            View.Property(p => p.TotalQty);
            View.Property(p => p.PassQty);
            View.Property(p => p.NgQty);
            View.Property(p => p.ScrapQty);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置台帐的库存详情视图
        /// </summary>
        void ConfigAccountListView()
        {
            View.AssignAuthorize(typeof(FixtureAccountModel));

            using (View.OrderProperties())
            {
                //View.UseDefaultCommands();
                View.Property(p => p.WarehouseCode).Show(ShowInWhere.All);
                View.Property(p => p.WarehouseName).Show(ShowInWhere.All);
                View.Property(p => p.LocationCode).Show(ShowInWhere.All);
                View.Property(p => p.LocationName).Show(ShowInWhere.All);
                View.Property(p => p.TotalQty).Show(ShowInWhere.All);
                View.Property(p => p.PassQty).Show(ShowInWhere.All);
                View.Property(p => p.NgQty).Show(ShowInWhere.All);
                View.Property(p => p.ScrapQty).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
