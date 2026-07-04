using SIE.Domain;
using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.Web.EMS.Purchases.EquipmentInbounds.Commands;

namespace SIE.Web.EMS.Purchases.EquipmentInbounds
{
    /// <summary>
    /// 设备入库视图配置
    /// </summary>
    internal class EquipmentInboundViewConfig : WebViewConfig<EquipmentInbound>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommand(typeof(WarehousingCommand).FullName);
            View.Property(p => p.InboundNo).ShowInList(130);
            View.Property(p => p.InboundType).ShowInList(120);
            View.Property(p => p.AcceptanceNo).ShowInList(130);
            View.Property(p => p.Qty).ShowInList(60);
            View.Property(p => p.EquipModelId).HasLabel("设备型号编码").ShowInList(130);
            View.Property(p => p.EquipModelName).ShowInList(120);
            View.Property(p => p.ReceiveType).ShowInList(80);
            View.Property(p => p.WarehouseId).ShowInList(120);
            View.Property(p => p.WorkshopId).ShowInList(120);
            View.Property(p => p.InboundStatus).HasLabel("状态").ShowInList(60);
            View.Property(p => p.InboundDateTime).ShowInList(150);
            View.Property(p => p.WarehouseOperatorId).ShowInList(80);
            View.Property(p => p.SupplierId).UseSupplierEditor().HasLabel("供应商编码").ShowInList(120);
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.CustomerId).HasLabel("客户编码").ShowInList(120);
            View.Property(p => p.CustomerName).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.DetailList).HasLabel("设备明细").HasOrderNo(1).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(EquipmentInboundDetail), e =>
            {
                var args = e as ChildPagingDataArgs;
                var parent = args.Parent as EquipmentInbound;
                if (parent == null)
                {
                    return new EntityList<EquipmentInboundDetail>();
                }

                return RT.Service.Resolve<EquipmentInboundController>().GetEquipmentInboundDetails(parent.Id,
                    args.SortInfo, args.PagingInfo);

            }).HasLabel("设备明细").HasOrderNo(1);
        }
    }
}