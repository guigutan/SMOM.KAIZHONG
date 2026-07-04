using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.Equipments.Enums;

namespace SIE.Web.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 批次明细视图配置
    /// </summary>
    internal class SparePartAcceptanceLotViewConfig : WebViewConfig<SparePartAcceptanceLot>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Purchases.SparePartAcceptances.SparePartAcceptLotBehavior");
            View.UseCommand("SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.OnekeyPassCommand");
            View.Property(p => p.PurchaseOrderNo).ShowInList(130).Readonly();
            View.Property(p => p.PurchaseOrderItemLineNo).ShowInList().Readonly();
            View.Property(p => p.PurchaseObjectType).ShowInList(80).Readonly();
            View.Property(p => p.Price).ShowInList(80).Readonly();
            View.Property(p => p.WarehouseName).ShowInList(120).Readonly();
            View.Property(p => p.LotNo).ShowInList(200).Readonly();
            View.Property(p => p.Qty).ShowInList(100).Readonly();
            View.Property(p => p.AcceptanceResult).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject)
                .HasLabel("验收状态").ShowInList(80);
            View.Property(p => p.PassQty).UseSpinEditor(p =>
            {
                p.MinValue = 0;
            }).Readonly(p => p.ApprovalStatus == ApprovalStatus.PendingReview || p.ApprovalStatus == ApprovalStatus.UnderReview || p.ApprovalStatus == ApprovalStatus.Audited
            || p.AcceptanceResult == SIE.Common.InspectionResult.Fail);
            View.Property(p => p.UnqualifiedQty).UseSpinEditor(p =>
            {
                p.MinValue = 0;
            }).Readonly(p => p.ApprovalStatus == ApprovalStatus.PendingReview || p.ApprovalStatus == ApprovalStatus.UnderReview || p.ApprovalStatus == ApprovalStatus.Audited
            || p.AcceptanceResult == SIE.Common.InspectionResult.Fail);
            View.Property(p => p.Remark).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}