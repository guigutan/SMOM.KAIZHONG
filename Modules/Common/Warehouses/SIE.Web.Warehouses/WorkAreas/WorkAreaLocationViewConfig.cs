using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 工作区与库位关系视图配置
    /// </summary>
    internal class WorkAreaLocationViewConfig : WebViewConfig<WorkAreaLocation>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.UseCommands(typeof(WorkAreaSelLocationCommand).FullName, typeof(WorkAreaLocDeleteCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.StorageLocationCode).Readonly();
                View.Property(p => p.StorageLocationName).Readonly();
                View.Property(p => p.WarehouseCode).Readonly();
                View.Property(p => p.AreaCode).Readonly();
                View.Property(p => p.StorageLocationState).Readonly();
            }
        }
    }
}