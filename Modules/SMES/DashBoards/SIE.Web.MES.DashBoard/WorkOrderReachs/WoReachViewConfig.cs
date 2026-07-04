using SIE.MES.DashBoard.WorkOrderReachs;

namespace SIE.Web.MES.DashBoard.TeamManagement
{
    public class WoReachViewConfig : WebViewConfig<WoReachReportViewModel>
    {
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.DashBoard.WorkOrderReachs.Commands.WoReachExport");
        }
    }
}
