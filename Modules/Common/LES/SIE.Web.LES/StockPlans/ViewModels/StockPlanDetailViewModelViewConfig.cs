using SIE.LES.StockPlans.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.StockPlans.ViewModels
{
    /// <summary>
    /// 齐套弹出视图
    /// </summary>
    internal class StockPlanDetailViewModelViewConfig : WebViewConfig<StockPlanDetailViewModel>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.BuyOnLoad).UseCheckEditor(p => { p.BoxLabel = "考虑在途库存"; }).ShowInDetail(hideLabel: true);
            View.Property(p => p.MakeOnLoad).UseCheckEditor(p => { p.BoxLabel = "考虑生产在制"; }).ShowInDetail(hideLabel: true);
        }
    }
}
