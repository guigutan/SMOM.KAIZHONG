using SIE.Dock.DockMaintains;
using SIE.MetaModel.View;
using SIE.Web.Dock.DockMaintains.Commands;

namespace SIE.Web.Dock.DockMaintains
{
    /// <summary>
    /// 月台维护适应仓库视图配置
    /// </summary>
    internal class DockMaintainWhViewConfig : WebViewConfig<DockMaintainWh>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SIE.Dock.DockMaintains.DockMaintain));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(SelectWarehouseCommand).FullName, typeof(DeleteDockWhCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseCode).Readonly();
                View.Property(p => p.WarehouseName).Readonly();
                View.Property(p => p.WarehouseState).Readonly();
            }
        }
    }
}