using SIE.Domain;
using SIE.EMS.InventoryPlans;
using SIE.Equipments;
using SIE.Resources.Enterprises;
using SIE.Web.Common;
using SIE.Web.Equipments.Extensions;
using System.Linq;
using System;
using SIE.Equipments.EquipTypes;
using SIE.EMS.Equipments;
using SIE.Equipments.EquipAccounts;
using SIE.Warehouses;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划范围（设备）视图配置
    /// </summary>
    public class InventoryPlanEquipmentViewConfig : WebViewConfig<InventoryPlanEquipment>
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

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.DisableEditing();
            View.UseDetail(4);
            View.Property(p => p.ManageDeptName);
            View.Property(p => p.UseDeptId);
            View.Property(p => p.WarehouseId);
            View.Property(p => p.WorkShopId);
            View.Property(p => p.TypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.ReloadDataOnPopping = true; });
            View.Property(p => p.EquipTypeId);
            View.Property(p => p.EquipModelId);
            View.Property(p => p.UseLevel).UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.ReloadDataOnPopping = true; });
            View.Property(p => p.IsAsset);
            View.Property(p => p.AssetsCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.ReloadDataOnPopping = true; });
            View.Property(p => p.AssetOwnerId);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(InventoryPlan));
            View.AddBehavior("SIE.Web.EMS.InventoryPlans.InventoryPlanEquipBehavior");
            View.UseDetail(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.ManageDeptId).UseFactoryDepartmentsEditor().Show();
                View.Property(p => p.UseDeptId).UseFactoryDepartmentsEditor().Show();
                View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
                }).Show();
                View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as InventoryPlanEquipment;
                    
                    if (entity == null)
                    {
                        return new EntityList<Enterprise>();
                    }
                    
                    var workshop = RT.Service.Resolve<EnterpriseController>().GetWorkShops(pagingInfo, keyword, entity.FactoryId);
                    
                    if (workshop == null || workshop.Count <= 0)
                    {
                        return new EntityList<Enterprise>();
                    }
                    
                    return workshop;
                }).Show();
                //更新设备类别时，清空设备类型和设备型号
                View.Property(p => p.TypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).Show()
                    .Cascade(p => p.EquipTypeId, null).Cascade(p => p.EquipModelId, null);

                //设备类别不为空时，可选项限制是设备类别下的设备类型
                View.Property(p => p.EquipTypeId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as InventoryPlanEquipment;
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
                }).Show().Cascade(p => p.EquipModelId, null);

                //设备类别和设备类型字段不为空时，可选项限制在设备类别和设备类型下的设备型号
                View.Property(p => p.EquipModelId).UseDataSource((e, c, r) =>
                {
                    var entity = e as InventoryPlanEquipment;
                    return RT.Service.Resolve<EquipController>().GetEquipModels(c, entity?.EquipTypeId, r);
                }).Show();
                View.Property(p => p.UseLevel).UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.ReloadDataOnPopping = true; }).Show();
                View.Property(p => p.IsAsset).Cascade(p => p.AssetsCategory, null).Cascade(p => p.AssetOwnerId, null).Show();
                View.Property(p => p.AssetsCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.ReloadDataOnPopping = true; })
                    .Readonly(p => p.IsAsset != YesNo.Yes).Show();
                View.Property(p => p.AssetOwnerId).Readonly(p => p.IsAsset != YesNo.Yes).Show();
            }
        }
    }
}