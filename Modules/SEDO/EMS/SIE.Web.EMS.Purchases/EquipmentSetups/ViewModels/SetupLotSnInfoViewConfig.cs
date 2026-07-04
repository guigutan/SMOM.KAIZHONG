using SIE.EMS.Purchases.EquipmentSetups.ViewModels;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.ViewModels
{
    /// <summary>
    /// 安装调试备件使用条码视图
    /// </summary>
    public class SetupLotSnInfoViewConfig : WebViewConfig<SetupLotSnInfo>
    {
        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Value).ShowInList(150);
        }
    }
}
