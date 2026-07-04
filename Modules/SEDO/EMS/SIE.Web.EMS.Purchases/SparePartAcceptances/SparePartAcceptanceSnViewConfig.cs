using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.Equipments.Enums;

namespace SIE.Web.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 序列号明细视图配置
    /// </summary>
    internal class SparePartAcceptanceSnViewConfig : WebViewConfig<SparePartAcceptanceSn>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand("SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.OnekeyPassCommand");
            View.Property(p => p.PurchaseOrderNo).ShowInList(130).Readonly();
            View.Property(p => p.PurchaseOrderItemLineNo).ShowInList().Readonly();
            View.Property(p => p.PurchaseObjectType).ShowInList(80).Readonly();
            View.Property(p => p.Price).ShowInList(80).Readonly();
            View.Property(p => p.WarehouseName).ShowInList(120).Readonly();
            View.Property(p => p.Sn).ShowInList(200).Readonly();
            View.Property(p => p.OriginalSn).ShowInList(200).Readonly();
            View.Property(p => p.AcceptanceResult).HasLabel("验收状态").ShowInList(80)
                .Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject);
            View.Property(p => p.Remark).ShowInList(200).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Sn).ShowInList(200).Readonly();
        }
    }
}