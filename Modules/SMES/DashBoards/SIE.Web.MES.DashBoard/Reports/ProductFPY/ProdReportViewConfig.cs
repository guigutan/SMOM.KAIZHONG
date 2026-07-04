using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ProductFPY;

namespace SIE.Web.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 产品报表视图配置
    /// </summary>
    internal class ProdReportViewConfig : WebViewConfig<ProductReportViewModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseCommands("SIE.Web.MES.DashBoard.Reports.ProductFPY.Commands.TargetParaCommand");
        }
    }
}
