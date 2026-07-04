using SIE.CSM.Suppliers;
using SIE.Web.ERPInterface.DownloadManual.Suppliers.Commands;

namespace SIE.Web.ERPInterface.DownloadManual.Suppliers
{
    /// <summary>
    /// 供应商扩展视图
    /// </summary>
    public class SupplierExtViewConfig : WebViewConfig<Supplier>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(DlSupplierCommand).FullName);
        }
    }
}
