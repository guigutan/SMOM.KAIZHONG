using SIE.Domain;
using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.Equipments.Enums;
using SIE.MetaModel.View;
using SIE.Warehouses;

namespace SIE.Web.EMS.Purchases.EquipmentInbounds
{
    /// <summary>
    /// 设备入库明细视图配置
    /// </summary>
    internal class EquipmentInboundDetailViewConfig : WebViewConfig<EquipmentInboundDetail>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentInbounds.Commands.SelectLocationCommand", WebCommandNames.Save);
            View.Property(p => p.Giveaway).Readonly().ShowInList(50);
            View.Property(p => p.PurchaseOrderId).HasLabel("采购单号").Readonly().ShowInList(130);
            View.Property(p => p.PurchaseOrderItemId).HasLabel("采购单行号").Readonly().ShowInList(100);
            View.Property(p => p.EquipmentCode).Readonly().ShowInList(130);
            View.Property(p => p.EquipAccountName).Readonly().ShowInList(200);
            View.Property(p => p.StorageLocationId).UseDataSource((o, e, r) =>
            {
                var model = o as EquipmentInboundDetail;
                if (model == null)
                {
                    return new EntityList<StorageLocation>();
                }
                return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(null, model.WarehouseId, r, e);
            }).Readonly(p => p.InboundStatus != InboundStatus.ToBe || p.WorkshopId != null);
            View.Property(p => p.UseLevel).Readonly().ShowInList(100);
            View.Property(p => p.UseDepartmentName).Readonly().ShowInList(100);
            View.Property(p => p.InstallationLocation).Readonly().ShowInList(100);
            View.Property(p => p.Price).Readonly().ShowInList(80);
            View.Property(p => p.OriginalSerialNumber).Readonly().ShowInList(130);
            View.Property(p => p.EnterDate).Readonly().ShowInList(150);
            View.Property(p => p.Manufacturer).Readonly().ShowInList(100);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.StorageLocationId).UseDataSource((o, e, r) =>
            {
                var model = o as EquipmentInboundDetail;
                if (model == null)
                {
                    return new EntityList<StorageLocation>();
                }
                return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(null, model.WarehouseId, r, e);
            });
        }
    }
}