using SIE.Items;
using SIE.MES.TaskManagement.Reports;

namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工打印设置
    /// </summary>
    public class ReportPrintConfigViewConfig : WebViewConfig<ReportPrintConfig>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProductFamily));
        }

        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Template).ShowInDetail().HasLabel("报工打印模板");
        }
    }
}
