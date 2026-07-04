using SIE.Core.Equipments;
using SIE.Domain;
using SIE.EMS.InventoryPlans;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Items;
using SIE.Warehouses;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>
    /// 工治具盘点范围
    /// </summary>
    public class InventoryPlanSparePartViewConfig : WebViewConfig<InventoryPlanSparePart>
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
            View.UseDetail(3);
            View.Property(p => p.Warehouses).ShowInDetail(columnSpan: 1);
            View.Property(p => p.StorageAreas).ShowInDetail(columnSpan: 1);
            View.Property(p => p.StorageLocations).ShowInDetail(columnSpan: 1);
            View.Property(p => p.PartType).ShowInDetail(columnSpan: 1);

            View.Property(p => p.ItemCategoryId).ShowInDetail(columnSpan: 1);
            View.Property(p => p.SparePartId).Show(ShowInWhere.Hide);
            View.Property(p => p.ControlMethod).Show(ShowInWhere.Hide);
            View.Property(p => p.IsFixAsset);

            View.Property(p => p.AssetsCategory).UseCatalogEditor(p => p.CatalogType = EquipType.EquipTypeCatalogType);
            View.Property(p => p.AssetOwnerId);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(InventoryPlan));
            View.UseDetail(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.Warehouses).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(SIE.EMS.InventoryPlans.Warehouse).FullName;
                    p.LinkField = InventoryPlanSparePart.WarehouseIdsProperty.Name;
                    p.DisplayField = SIE.EMS.InventoryPlans.Warehouse.CodeProperty.Name;
                    p.XType = "IpSpMultiWhComboPopup";
                    p.Editable = false;
                    p.Separator = ";";
                }).ShowInDetail(columnSpan: 1).HasLabel("仓库".L10N() + "*");

                View.Property(p => p.StorageAreas).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(StorageArea).FullName;
                    p.LinkField = InventoryPlanSparePart.StorageAreaIdsProperty.Name;
                    p.DisplayField = StorageArea.CodeProperty.Name;
                    p.XType = "IpSpMultiAreaComboPopup";
                    p.Editable = false;
                    p.Separator = ";";
                }).ShowInDetail(columnSpan: 1).Readonly(x => x.Warehouses == String.Empty);

                View.Property(p => p.StorageLocations).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(StorageLocation).FullName;
                    p.LinkField = InventoryPlanSparePart.StorageLocationIdsProperty.Name;
                    p.DisplayField = StorageLocation.CodeProperty.Name;
                    p.XType = "IpSpMultiLocComboPopup";
                    p.Editable = false;
                    p.Separator = ";";
                }).ShowInDetail(columnSpan: 1).Readonly(x => x.Warehouses == String.Empty);

                View.Property(p => p.PartType).ShowInDetail(columnSpan: 1).Cascade(p => p.SparePartId, null);

                View.Property(p => p.ItemCategoryId).UseDataSource((e, p, k) =>
                {
                    var list = RT.Service.Resolve<ItemController>().GetItemCategoryByItemType(ItemType.SparePart, SIE.Items.Items.CategoryType.Item, k, p);
                    var itemIds = list.Select(p => p.Id).ToList();
                    foreach (var item in list)
                    {
                        if (!itemIds.Contains(item.TreePId ?? 0))
                        {
                            item.TreePId = null;
                        }
                    }
                    return list;
                }).ShowInDetail(columnSpan: 1).Cascade(p => p.SparePartId, null);
                View.Property(p => p.SparePartId)
                    .UseDataSource((e, p, k) => {
                        var inventoryPlanSparePart =e as InventoryPlanSparePart;
                        if (inventoryPlanSparePart == null)
                        {
                            return new EntityList<SparePart>();
                        }
                        else
                        {
                            return RT.Service.Resolve<SparePartController>()
                                .GetSparePartList(inventoryPlanSparePart.PartType, inventoryPlanSparePart.ItemCategoryId, k, p);
                        }
                    })
                    .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                    m.DicLinkField = keyValues;
                }).Show(ShowInWhere.Hide);
                View.Property(p => p.ControlMethod).ShowInDetail(columnSpan: 1).Readonly(p => p.SparePartId != null)
                    .Cascade(x => x.IsFixAsset, null).Show(ShowInWhere.Hide);
                View.Property(p => p.IsFixAsset).ShowInDetail().Readonly(x => x.ControlMethod == null || x.ControlMethod != ControlMethod.Sn);

                View.Property(p => p.AssetsCategory).UseCatalogEditor(p => p.CatalogType = EquipType.EquipTypeCatalogType)
                    .Readonly(p => p.IsFixAsset != YesNo.Yes).Show();
                View.Property(p => p.AssetOwnerId).Readonly(p => p.IsFixAsset != YesNo.Yes).Show();

            }
        }
    }
}
