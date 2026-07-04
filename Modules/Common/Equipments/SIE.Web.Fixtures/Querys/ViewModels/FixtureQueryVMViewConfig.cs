using SIE.Fixtures.Querys.ViewModels;

namespace SIE.Web.Fixtures.Querys.ViewModels
{
    /// <summary>
    /// 配置工治具查询ViewModel视图
    /// </summary>
    internal class FixtureQueryVMViewConfig : WebViewConfig<FixtureQueryViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Fixtures.Querys.Scripts.FixtureQueryBehavior");
            View.ClearCommands();
            View.UseCommands("SIE.Web.Fixtures.Querys.Commands.UnloadCommand");
            View.Property(p => p.EncodeCode).Readonly();
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.WarehouseName).Readonly();
            View.Property(p => p.LocationName).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.RepairBeforeState).UseEnumEditor().Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.WorkOrderNo).Readonly();
        }
    }
}
