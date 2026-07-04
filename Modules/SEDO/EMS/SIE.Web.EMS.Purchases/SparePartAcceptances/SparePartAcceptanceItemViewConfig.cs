using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.Equipments.Enums;

namespace SIE.Web.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收项目视图配置
    /// </summary>
    internal class SparePartAcceptanceItemViewConfig : WebViewConfig<SparePartAcceptanceItem>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.AddAcceptItemCommand", "SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.EditAcceptItemCommand",
                "SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.CopyAcceptItemCommand", "SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.DeleteAcceptItemCommand");
            View.Property(p => p.ItemName).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject).ShowInList(400);
            View.Property(p => p.AcceptanceValue).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject).ShowInList(400);
            View.Property(p => p.AcceptanceResult).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject).ShowInList(100);
            View.Property(p => p.Remark).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}