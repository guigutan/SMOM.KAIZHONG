using SIE.LES.LesStockCounts.ViewModels;
using SIE.Web.LES.Extensions;

namespace SIE.Web.LES.LesStockCounts.ViewModels
{
    /// <summary>
    /// 线边仓盘点打印视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class LesStockCountPrintViewModelViewConfig : WebViewConfig<LesStockCountPrintViewModel>
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
                    .UseBillPrintEditor().HasLabel("打印模板");
            }
        }
    }
}
