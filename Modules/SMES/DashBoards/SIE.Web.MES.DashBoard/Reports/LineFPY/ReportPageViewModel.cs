using SIE.Domain;
using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;

namespace SIE.Web.MES.DashBoard.Reports.LineFPY
{
    /// <summary>
    /// 报表自定义页面
    /// 为了解决克制化界面js加载问题
    /// </summary>
    [RootEntity]
    public class ReportPageViewModel : ViewModel
    {
    }

    /// <summary>
    /// 报表自定义页面视图配置
    /// </summary>
    internal class ReportPageViewModelViewConfig : WebViewConfig<ReportPageViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LineReportViewModel), typeof(ProductReportViewModel), typeof(ShopReportViewModel));
            View.RequirModuleResource("SIE.Web.MES.DashBoard.Reports.Common.Scripts.FpyReportPage.js");
        }
    }
}