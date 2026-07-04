using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;

namespace SIE.Web.MES.DashBoard.TeamManagement
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class ShopReportViewConfig : WebViewConfig<ShopReportViewModel>
    {
        /// <summary>
        /// list 视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.DashBoard.Reports.ShopFPY.Commands.TargetParaCommand");
        }
    }
}
