using SIE.Domain;
using SIE.EMS.AssetTransfers;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.Equipments.Extensions;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetTransfers
{
    /// <summary>
    /// 设备清单视图配置
    /// </summary>
    public class AssetTransferDetailViewConfig : WebViewConfig<AssetTransferDetail>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                DetailListView();
            }
        }
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountId).HasLabel("设备编码").Readonly().ShowInList(120) ;
                View.Property(p => p.EquipAccountName).Readonly().ShowInList(120);
                View.Property(p => p.Alias).Readonly().ShowInList(80);
                View.Property(p => p.EquipAccountModelCode).Readonly().ShowInList(80);
                View.Property(p => p.EquipAccountModelName).Readonly().ShowInList(80);
                
                View.Property(p => p.Specifications).Readonly().ShowInList(80);
                View.Property(p => p.FixedAssetCode).Readonly().ShowInList(120);
                View.Property(p => p.FixedAssetName).Readonly().ShowInList(120);

                View.Property(p => p.ResponsibleId).HasLabel("责任人").ShowInList(80);
                View.Property(p => p.WorkshopId).HasLabel("车间").ShowInList(80);
                View.Property(p => p.ResourceId).HasLabel("产线").ShowInList(80);
                View.Property(p => p.Location).HasLabel("位置").ShowInList(80);
                View.Property(p => p.WarehouseId).HasLabel("仓库").ShowInList(80);
                View.Property(p => p.StorageLocationId).HasLabel("库位").ShowInList(80);
                View.Property(p => p.KeeperId).HasLabel("保管人").ShowInList(80);

                View.Property(p => p.OriginalResponsibleId).HasLabel("原责任人").ShowInList(120);
                View.Property(p => p.OriginalWorkshopId).HasLabel("原车间").ShowInList(100);
                View.Property(p => p.OriginalResourceId).HasLabel("原产线").ShowInList(100);
                View.Property(p => p.OriginalLocation).HasLabel("原位置").ShowInList(80);
                View.Property(p => p.OriginalWarehouseId).HasLabel("原仓库").ShowInList(80);
                View.Property(p => p.OrinialStorageLocationId).HasLabel("原库位").ShowInList(80);
                View.Property(p => p.OrignalKeeperId).HasLabel("原保管人").ShowInList(100);
            }
        }

        /// <summary>
        /// 主表明细修改列表
        /// </summary>
        protected void DetailListView()
        {
            View.UseCommands("SIE.Web.EMS.AssetTransfers.Commands.AddDetailCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountId).UseDataSource((e, page, code) =>
                {
                    var entity = e as AssetTransferDetail;
                    if (entity == null)
                    {
                        return new EntityList<EquipAccountSelect>();
                    }
                    return RT.Service.Resolve<AssetTransfersController>().GetEquipAccounts(entity, page, code);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();

                    keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    keyValues.Add(nameof(e.EquipAccountModelCode), nameof(e.EquipAccount.ModelCode));
                    keyValues.Add(nameof(e.EquipAccountModelName), nameof(e.EquipAccount.ModelName));
                    keyValues.Add(nameof(e.Specifications), nameof(e.EquipAccount.Specifications));
                    keyValues.Add(nameof(e.FixedAssetCode), nameof(e.EquipAccount.FixedAssetsAccountCode));
                    keyValues.Add(nameof(e.FixedAssetName), nameof(e.EquipAccount.FixedAssetsAccountName)); 
                    keyValues.Add(nameof(e.Alias), nameof(e.EquipAccount.Alias));
                    keyValues.Add("OriginalResponsibleId_Display", nameof(e.EquipAccount.ResPersonName));
                    keyValues.Add(nameof(e.OriginalResponsibleId), nameof(e.EquipAccount.ResPersonId));
                    keyValues.Add("OriginalWorkshopId_Display", nameof(e.EquipAccount.WorkShopCode));
                    keyValues.Add(nameof(e.OriginalWorkshopId), nameof(e.EquipAccount.WorkShopId));

                    keyValues.Add("OriginalResourceId_Display", nameof(e.EquipAccount.ResourceName));
                    keyValues.Add(nameof(e.OriginalResourceId), nameof(e.EquipAccount.ResourceId));
                    keyValues.Add(nameof(e.OriginalLocation), nameof(e.EquipAccount.InstallationLocation));
                    keyValues.Add("OriginalWarehouseId_Display", nameof(e.EquipAccount.WarehouseName));
                    keyValues.Add(nameof(e.OriginalWarehouseId), nameof(e.EquipAccount.WarehouseId));

                    keyValues.Add("OrinialStorageLocationId_Display", nameof(e.EquipAccount.StorageLocationName));
                    keyValues.Add(nameof(e.OrinialStorageLocationId), nameof(e.EquipAccount.StorageLocationId));
                    keyValues.Add("OrignalKeeperId_Display", nameof(e.EquipAccount.AdministratorName));
                    keyValues.Add(nameof(e.OrignalKeeperId), nameof(e.EquipAccount.AdministratorId));
                   

                    m.DicLinkField = keyValues;
                }).ShowInList(120).HasLabel("设备编码");

                View.Property(p => p.EquipAccountName).Readonly().ShowInList(120);
                View.Property(p => p.EquipAccountModelCode).Readonly().ShowInList(120);
                View.Property(p => p.EquipAccountModelName).Readonly().ShowInList(120);
                View.Property(p => p.Specifications).Readonly().ShowInList(80);
                View.Property(p => p.FixedAssetCode).Readonly().ShowInList(120);
                View.Property(p => p.FixedAssetName).Readonly().ShowInList(120);

                View.Property(p => p.ResponsibleId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
                }).HasLabel("责任人").ShowInList(80);
                View.Property(p => p.WorkshopId).UseFactoryWorkshopEditor(factoryIdPropertyName: "ParentTargetFactoryId").Cascade(p => p.ResourceId, null).HasLabel("车间").ShowInList(80);
                   
                View.Property(p => p.ResourceId).UseWorkShopResourceEditor( workShopIdPropertyName:"WorkshopId").HasLabel("产线").ShowInList(80);
                View.Property(p => p.Location).HasLabel("位置").ShowInList(80);
                View.Property(p => p.Warehouse).UseDataSource((e, page, code) =>
                {
                    var entity = e as AssetTransferDetail;
                    if (entity == null)
                    {
                        return new EntityList<Warehouse>();
                    }
                    return RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployee(page, code);
                }).HasLabel("仓库").ShowInList(80);
                View.Property(p => p.StorageLocation).UseDataSource((e, page, code) =>
                {
                    var entity = e as AssetTransferDetail;
                    if (entity == null || !entity.WarehouseId.HasValue)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(entity.WarehouseId.Value, code, page);
                }).HasLabel("库位").ShowInList(80);
                View.Property(p => p.KeeperId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
                }).HasLabel("保管人").ShowInList(80);

                View.Property(p => p.OriginalResponsibleId).HasLabel("原责任人").ShowInList(120).Readonly();
                View.Property(p => p.OriginalWorkshopId).HasLabel("原车间").ShowInList(80).Readonly();
                View.Property(p => p.OriginalResourceId).HasLabel("原产线").ShowInList(80).Readonly();
                View.Property(p => p.OriginalLocation).HasLabel("原位置").ShowInList(80).Readonly();
                View.Property(p => p.OriginalWarehouseId).HasLabel("原仓库").ShowInList(80).Readonly();
                View.Property(p => p.OrinialStorageLocationId).HasLabel("原库位").ShowInList(80).Readonly();
                View.Property(p => p.OrignalKeeperId).HasLabel("原保管人").ShowInList(80).Readonly();
            }

        }
    }
}