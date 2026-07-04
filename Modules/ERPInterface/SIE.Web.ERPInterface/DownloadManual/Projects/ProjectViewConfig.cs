using SIE.Core.ProjectMaintains;
using SIE.Web.ERPInterface.DownloadManual.Suppliers.Commands;
using SIE.WMS.Common;

namespace SIE.Web.ERPInterface.DownloadManual.Suppliers
{
    /// <summary>
    /// 任务扩展视图
    /// </summary>
    public class ProjectExtViewConfig : WebViewConfig<ProjectMaintain>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseCommands(typeof(DlTaskNoCommand).FullName);
        }
    }
}
