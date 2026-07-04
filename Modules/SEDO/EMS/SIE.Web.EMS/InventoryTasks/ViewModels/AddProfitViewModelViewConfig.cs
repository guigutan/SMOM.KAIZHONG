using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.InventoryTasks;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.Equipments;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.Equipments.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Web.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 新增盘盈界面
    /// </summary>
    internal class AddProfitViewModelViewConfig : WebViewConfig<AddProfitViewModel>
    {
        /// <summary>
        /// 明细界面
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //进入界面，界面为C状态，除了设备编码和无设备编码可以输入或勾选，其他字段全部为空不可编辑
            Expression<Func<AddProfitViewModel, bool>> exp = p => p.AddProfitUIState == AddProfitUIState.C;
            //B状态时可输入
            Expression<Func<AddProfitViewModel, bool>> expB = p => p.AddProfitUIState != AddProfitUIState.B;

            View.AssignAuthorize(typeof(InventoryTask));
            View.UseCommand("SIE.Web.EMS.InventoryTasks.Commands.AddProfitQueryCommand");
            View.UseDetail(8);
            View.Property(p => p.EquipmentCode).Readonly(p => p.NoHaveCode).ShowInDetail(columnSpan: 2);
            View.Property(p => p.NoHaveCode).ShowInDetail(columnSpan: 6);
            View.Property(p => p.EquipmentName).Readonly(expB).ShowInDetail(columnSpan: 2);
            View.Property(p => p.AccountUseState).Readonly(exp).ShowInDetail(columnSpan: 2);
            View.Property(p => p.AccountState).Readonly(exp).ShowInDetail(columnSpan: 2);
            View.Property(p => p.ManageDeptName).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.TypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.ReloadDataOnPopping = true; }).Readonly(expB)
                .Cascade(p => p.EquipTypeId, null).Cascade(p => p.EquipModelId, null).Cascade(p => p.EquipModelName, null
                ).Cascade(p => p.Specifications, null).ShowInDetail(columnSpan: 2);

            //B状态时可选设备类别下的设备类型,设备类别为空时，选择后带出设备类别
            View.Property(p => p.EquipTypeId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as AddProfitViewModel;
                if (entity == null)
                {
                    return new EntityList<EquipType>();
                }
                if (!entity.TypeCategory.IsNullOrWhiteSpace())
                {
                    return RT.Service.Resolve<CoreEquipController>().GetEquipTypes(entity.TypeCategory, pagingInfo, keyword);
                }
                else
                {
                    return RT.Service.Resolve<CoreEquipController>().GetEquipTypes(pagingInfo, keyword);
                }
            }).Readonly(expB).Cascade(p => p.EquipModelId, null).Cascade(p => p.EquipModelName, null
                ).Cascade(p => p.Specifications, null).ShowInDetail(columnSpan: 2);

            //设备类别和设备类型为空时，选择后带出设备类别和设备类别
            View.Property(p => p.EquipModelId).UseDataSource((e, c, r) =>
            {
                var entity = e as AddProfitViewModel;
                if (entity == null)
                {
                    return new EntityList<EquipModel>();
                }
                else
                {
                    return RT.Service.Resolve<EquipController>().GetEquipModels(c, entity?.EquipTypeId, r, entity.TypeCategory);
                }

            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
                keyValues.Add(nameof(e.Specifications), nameof(e.EquipModel.Specifications));
                m.DicLinkField = keyValues;
            }).Readonly(expB).ShowInDetail(columnSpan: 2);
            View.Property(p => p.EquipModelName).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.Specifications).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.UseLevel).UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.ReloadDataOnPopping = true; }).Readonly(exp).ShowInDetail(columnSpan: 2);
            View.Property(p => p.UseDeptId).UseFactoryDepartmentsEditor().Readonly(exp).ShowInDetail(columnSpan: 2);
            View.Property(p => p.UserId).Readonly(exp).ShowInDetail(columnSpan: 2);
            View.Property(p => p.RealWorkShopId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as AddProfitViewModel;
                if (entity == null)
                {
                    return new EntityList<Enterprise>();
                }
                var workshop = RT.Service.Resolve<EnterpriseController>().GetWorkShops(pagingInfo, keyword, entity.FactoryId);
                if (workshop == null || workshop.Count <= 0)
                {
                    return new EntityList<Enterprise>();
                }
                workshop.ForEach(p => p.TreePId = null);
                return workshop;
            }).Readonly(exp).Cascade(p => p.RealResourceId, null).ShowInDetail(columnSpan: 2);
            View.Property(p => p.RealResourceId).UseDataSource((e, c, r) =>
            {
                var entity = e as AddProfitViewModel;
                if (entity == null || entity.RealWorkShopId == null)
                    return new EntityList<Enterprise>();
                var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, entity.RealWorkShopId.Value);
                resourcesList.ForEach(p => p.TreePId = null);
                return resourcesList;
            }).Readonly(exp).ShowInDetail(columnSpan: 2);
            View.Property(p => p.RealWarehouseId).Readonly(exp).Cascade(p => p.StorageLocationId, null).ShowInDetail(columnSpan: 2);
            View.Property(p => p.StorageLocationId).UseDataSource((o, e, r) =>
            {
                var model = o as AddProfitViewModel;
                if (model == null || model.RealWarehouseId == null)
                {
                    return new EntityList<StorageLocation>();
                }
                return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(null, model.RealWarehouseId, r, e);
            }).Readonly(exp).ShowInDetail(columnSpan: 2);
            View.Property(p => p.RealLocation).Readonly(exp).ShowInDetail(columnSpan: 4);
            View.Property(p => p.PhotoFilePath).UseConfigValueEditor(p =>
            {
                p.XType = "uploadinventoryplanphotoeditor";
                p.AllowBlank = true;
                p.Editable = false;
            }).Readonly(exp).ShowInDetail(columnSpan: 4);
        }
    }
}
