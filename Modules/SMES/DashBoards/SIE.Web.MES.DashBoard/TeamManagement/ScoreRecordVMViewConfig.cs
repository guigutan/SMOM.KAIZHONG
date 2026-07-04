using SIE.MES.DashBoard.TeamManagement;

namespace SIE.Web.MES.DashBoard.TeamManagement
{
    public class ScoreRecordVMViewConfig : WebViewConfig<ScoreRecordViewModel>
    {
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.DashBoard.TeamManagement.Commands.ScoreRecordExport");
        }
    }
}
