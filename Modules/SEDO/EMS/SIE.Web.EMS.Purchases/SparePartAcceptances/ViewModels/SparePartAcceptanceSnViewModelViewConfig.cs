using SIE.EMS.Purchases.SparePartAcceptances.ViewModels;

namespace SIE.Web.EMS.Purchases.SparePartAcceptances.ViewModels
{
    internal class SparePartAcceptanceSnViewModelViewConfig : WebViewConfig<SparePartAcceptanceSnViewModel>
    {
        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Sn).ShowInList(200).Readonly();
        }
    }
}
