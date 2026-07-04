using SIE.EMS.Purchases.EquipmentReceives;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收序列号模型界面
    /// </summary>
    internal class ReceiveScanSnViewModelViewConfig : WebViewConfig<ReceiveScanSnViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand("SIE.Web.EMS.Purchases.EquipmentReceives.Commands.DeleteReceiveScanSnCommand");
            View.DisableEditing();
            View.Property(p => p.ReceiveLineNo).ShowInList(130);
            View.Property(p => p.PurchaseOrderNo).ShowInList(130);
            View.Property(p => p.OrderItemNo).ShowInList(100);
            View.Property(p => p.EquipModelCode).ShowInList(120);
            View.Property(p => p.EquipModelName).ShowInList(150);
            View.Property(p => p.TechnicalNorm).ShowInList(100);
            View.Property(p => p.EquipmentCode).ShowInList(130);
            View.Property(p => p.OriginalSn).ShowInList(130);
        }
    }
}
