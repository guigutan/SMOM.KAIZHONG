using SIE.MES.TaskManagement.Reports;

namespace SIE.Web.MES.TaskManagement.Reports
{
    internal class ReportDispatchTaskViewModelViewConfig
    {
#pragma warning disable S1144
        /// <summary>
        /// 产品报表视图配置
        /// </summary>
        internal class ProdReportViewConfig : WebViewConfig<ReportDispatchTaskViewModel>
        {
            /// <summary>
            /// 默认视图
            /// </summary>
            protected override void ConfigView()
            {
                // View.UseCommands("SIE.Web.MES.DashBoard.Reports.ProductFPY.Commands.TargetParaCommand");
            }
        }
#pragma warning restore S1144
    }
}
