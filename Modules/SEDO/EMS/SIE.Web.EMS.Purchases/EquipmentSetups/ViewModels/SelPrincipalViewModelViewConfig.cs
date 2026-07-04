using SIE.EMS.Purchases.EquipmentSetups.ViewModels;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.ViewModels
{
    /// <summary>
    /// 选择员工界面
    /// </summary>
    internal class SelPrincipalViewModelViewConfig : WebViewConfig<SelPrincipalViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.PrincipalId);
        }
    }
}
