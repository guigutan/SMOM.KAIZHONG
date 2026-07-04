using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.Warehouses;

namespace SIE.Web.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收明细视图配置
    /// </summary>
    internal class SparePartAcceptanceDetailViewConfig : WebViewConfig<SparePartAcceptanceDetail>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Purchases.SparePartAcceptances.SparePartAcceptDetBehavior");
            View.Property(p => p.PurchaseOrderId).HasLabel("采购单号").ShowInList(130).Readonly();
            View.Property(p => p.PurchaseOrderItemId).HasLabel("采购单行号").ShowInList(100).Readonly();
            View.Property(p => p.PurchaseObjectType).ShowInList(80).Readonly();
            View.Property(p => p.Price).ShowInList(80).Readonly();
            View.Property(p => p.WarehouseId).UsePagingLookUpEditor((m, e) =>
            {
                m.DisplayField = Warehouse.NameProperty.Name;
                m.BindDisplayField = SparePartAcceptanceDetail.WarehouseNameProperty.Name;
            }).HasLabel("接收仓库").ShowInList(120).Readonly();
            View.Property(p => p.ReceiveQty).ShowInList(100).Readonly();
            View.Property(p => p.UnitName).ShowInList(60).Readonly();
            View.Property(p => p.PassQty).UseSpinEditor(p => p.MinValue = 0).ShowInList(100)
                .Readonly(p => p.ControlMethod != ControlMethod.ItemCode || p.ApprovalStatus == ApprovalStatus.PendingReview
                || p.ApprovalStatus == ApprovalStatus.UnderReview || p.ApprovalStatus == ApprovalStatus.Audited);
            View.Property(p => p.UnqualifiedQty).UseSpinEditor(p => p.MinValue = 0).ShowInList(100)
                .Readonly(p => p.ControlMethod != ControlMethod.ItemCode || p.ApprovalStatus == ApprovalStatus.PendingReview
                || p.ApprovalStatus == ApprovalStatus.UnderReview || p.ApprovalStatus == ApprovalStatus.Audited);
            View.Property(p => p.Remark).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject).ShowInList(100);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.LotList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.SnList).Show(ChildShowInWhere.Hide);
        }
    }
}