using SIE.EMS.Purchases.EquipmentAcceptances;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 设备开箱验收明细视图配置
    /// </summary>
    public class EquipmentAcceptanceDetailViewConfig : WebViewConfig<EquipmentAcceptanceDetail>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.PurchaseOrderId).HasLabel("采购单号").ShowInList(130);
            View.Property(p => p.PurchaseOrderItemId).HasLabel("采购单行号").ShowInList(100);
            View.Property(p => p.EquipmentCode).ShowInList(130);
            View.Property(p => p.Giveaway).Readonly().ShowInList(50);
            View.Property(p => p.Price).ShowInList(80);
            View.Property(p => p.OriginalSn).ShowInList(130);
            View.Property(p => p.AcceptanceStatus).ShowInList(80);
            View.Property(p => p.WarehouseId).ShowInList(120);
            View.Property(p => p.WorkshopId).ShowInList(120);
            View.Property(p => p.Remark).ShowInList(100);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(EquipmentAcceptance));
            View.AddBehavior("SIE.Web.EMS.Purchases.EquipmentAcceptances.EquipAcceptDetailBehavior");
            View.UseCommand("SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.OnekeyPassCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.PurchaseOrderId).HasLabel("采购单号").ShowInList(130).Readonly();
                View.Property(p => p.PurchaseOrderItemId).HasLabel("采购单行号").ShowInList(100).Readonly();
                View.Property(p => p.EquipmentCode).ShowInList(130).Readonly();
                View.Property(p => p.Giveaway).Readonly().ShowInList(50).Readonly();
                View.Property(p => p.Price).ShowInList(80).Readonly();
                View.Property(p => p.OriginalSn).ShowInList(130).Readonly();
                View.Property(p => p.AcceptanceStatus).ShowInList(80);
                View.Property(p => p.WarehouseId).ShowInList(120).Readonly();
                View.Property(p => p.WorkshopId).ShowInList(120).Readonly();
                View.Property(p => p.Remark).ShowInList(100);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}