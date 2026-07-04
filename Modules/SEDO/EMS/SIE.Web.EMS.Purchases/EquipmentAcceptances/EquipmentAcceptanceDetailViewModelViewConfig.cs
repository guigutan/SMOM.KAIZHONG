using SIE.EMS.Purchases.EquipmentAcceptances.ViewModels;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 设备开箱验收明细视图的视图配置
    /// </summary>
    internal class EquipmentAcceptanceDetailViewModelViewConfig : WebViewConfig<EquipmentAcceptanceDetailViewModel>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.EquipmentCode);
        }
    }
}