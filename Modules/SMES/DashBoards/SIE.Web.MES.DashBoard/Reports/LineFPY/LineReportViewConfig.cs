using SIE.MES.DashBoard.Reports.LineFPY;

namespace SIE.Web.MES.DashBoard.Reports.LineFPY
{
    /// <summary>
    /// 产线报表视图配置
    /// </summary>
    internal class LineReportViewConfig : WebViewConfig<LineReportViewModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.MES.DashBoard.Reports.LineFPY.Commands.TargetParaCommand");
        }
    }
}
