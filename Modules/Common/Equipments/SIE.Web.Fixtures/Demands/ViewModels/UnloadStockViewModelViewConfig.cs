using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.Web.Fixtures.Demands.Commands;

namespace SIE.Web.Fixtures.Demands.ViewModels
{
    /// <summary>
    /// 配置库存情况ViewModel视图
    /// </summary>
    internal class UnloadStockViewModelViewConfig : WebViewConfig<UnloadStockViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(FixtureDemand));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.WithoutPaging();
            View.UseCommands(typeof(UnloadStockCommand).FullName);
            View.Property(p => p.WarehouseCode).Readonly();
            View.Property(p => p.WarehouseName).Readonly();
            View.Property(p => p.LocationCode).Readonly();
            View.Property(p => p.LocationName).Readonly();
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.Qty).Readonly();
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.Property(p => p.WarehouseCode).Readonly();
            View.Property(p => p.WarehouseName).Readonly();
            View.Property(p => p.LocationCode).Readonly();
            View.Property(p => p.LocationName).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.UnloadQty).UseSpinEditor(p =>
            {
                p.AllowDecimals = false;
                p.MinValue = 0;
            });
            View.Property(p => p.TurnoverToolCode);
        }
    }
}
