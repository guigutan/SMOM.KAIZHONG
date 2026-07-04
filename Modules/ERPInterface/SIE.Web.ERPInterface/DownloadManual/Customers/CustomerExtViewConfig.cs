using SIE.CSM.Customers;
using SIE.Web.ERPInterface.DownloadManual.Customers.Commands;

namespace SIE.Web.ERPInterface.DownloadManual.Customers
{
    /// <summary>
    /// 客户扩展视图
    /// </summary>
    public class CustomerExtViewConfig : WebViewConfig<Customer>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(DlCustomerCommand).FullName);
        }
    }
}