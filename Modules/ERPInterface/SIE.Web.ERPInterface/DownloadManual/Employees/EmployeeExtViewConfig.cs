using SIE.Common.Employees;
using SIE.Web.ERPInterface.DownloadManual.Employees.Commands;

namespace SIE.Web.ERPInterface.DownloadManual.Employees
{
    /// <summary>
    /// 客户扩展视图
    /// </summary>
    public class EmployeeExtViewConfig : WebViewConfig<Employee>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(DlEmployeeCommand).FullName);
        }
    }
}