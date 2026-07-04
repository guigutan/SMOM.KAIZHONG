using SIE.Common.Prints;
using SIE.LES.Distributions.Printables;
using SIE.Warehouses;
using System;

namespace SIE.Web.LES.Distributions
{
    /// <summary>
    /// 配送单打印 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class DistributionPrintViewModelViewConfig : WebViewConfig<DistributionPrintViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.BillTemplate).Show(ShowInWhere.Detail)
                    .UseDataSource((source, pagingInfo, keyword) =>
                    {
                        string qualifiedName = typeof(DistributionBillPrintable).GetQualifiedName();
                        return RT.Service.Resolve<WarehouseController>().GetPrintTemplatesByType(qualifiedName, keyword, pagingInfo, PrintType.Bill);
                    }).HasLabel("打印模板");
            }
        }
    }
}
