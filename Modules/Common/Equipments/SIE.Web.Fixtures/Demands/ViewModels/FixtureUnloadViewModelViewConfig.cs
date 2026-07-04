using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.Web.Fixtures.Demands.Commands;

namespace SIE.Web.Fixtures.Demands.ViewModels
{
    /// <summary>
    /// 配置出库明细ViewModel视图
    /// </summary>
    internal class FixtureUnloadViewModelViewConfig : WebViewConfig<FixtureUnloadViewModel>
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
            View.UseCommands("SIE.Web.Fixtures.Demands.Commands.UnloadEditCommand", "SIE.Web.Fixtures.Demands.Commands.UnloadDeleteCommand", typeof(UnloadSaveCommand).FullName);
            View.Property(p => p.WarehouseCode).Readonly();
            View.Property(p => p.WarehouseName).Readonly();
            View.Property(p => p.LocationCode).Readonly();
            View.Property(p => p.LocationName).Readonly();
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.UnloadQty).UseSpinEditor(p =>
            {
                p.AllowDecimals = false;
                p.MinValue = 0;
            }).Readonly(p => p.IsOld);
            View.Property(p => p.NgQty).ShowInList(width: 150).Readonly();
            View.Property(p => p.TurnoverToolCode).Readonly();
            View.Property(p => p.UnloadByName).Readonly();
            View.Property(p => p.UnloadDate).Readonly();
        }
    }
}
