using SIE.EMS.Purchases.EquipmentReceives;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 序列号明细视图配置
    /// </summary>
    internal class EquipmentReceiveSnViewConfig : WebViewConfig<EquipmentReceiveSn>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.WithoutPaging();
            View.UseGridSelectionModel();
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentReceives.Commands.ScreenCommand", "SIE.Web.EMS.Purchases.EquipmentReceives.Commands.ReceiveSnPrintCommand");
            View.Property(p => p.PurchaseOrderNo).ShowInList(130);
            View.Property(p => p.OrderItemNo).ShowInList(100);
            View.Property(p => p.EquipModelCode).ShowInList(120);
            View.Property(p => p.EquipModelName).ShowInList(150);
            View.Property(p => p.Giveaway).ShowInList(50);
            View.Property(p => p.EquipmentCode).ShowInList(130);
            View.Property(p => p.OriginalSn).ShowInList(130);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}