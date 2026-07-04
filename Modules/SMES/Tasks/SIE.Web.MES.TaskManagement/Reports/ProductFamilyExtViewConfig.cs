using SIE.Items;
using SIE.MES.TaskManagement.Reports;

namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 产品族扩展属性
    /// </summary>
    internal class ProductFamilyExtViewConfig : WebViewConfig<ProductFamily>
    {
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.MES.TaskManagement.Reports.ProductFamilyBehavior");
            View.AssociateChildrenProperty(FamilyExtReportPrint.FamilyPrintRuleProperty, (e) =>
            {
                var family = e.Parent as ProductFamily;
                if (family == null)
                    return new ReportPrintConfig();
                var printConfig = RT.Service.Resolve<ReportController>().GetReportPrintConfig(family.Id);
                if (printConfig == null)
                {
                    printConfig = new ReportPrintConfig();
                    printConfig.GenerateId();
                }
                return printConfig;
            }, ReportPrintConfigViewConfig.DetailsView).HasLabel("报工打印设置").OrderNo = 32;
        }
    }
}
