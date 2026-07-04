using SIE.Warehouses;
using SIE.Web.ERPInterface.DownloadManual.Warehouses.Commands;

namespace SIE.Web.ERPInterface.DownloadManual.Warehouses
{
    /// <summary>
    /// 仓库扩展视图
    /// </summary>
    public class ErpWarehouseExtViewConfig : WebViewConfig<ErpWarehouse>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(DlErpWarehouseCommand).FullName);
        }
    }
}
