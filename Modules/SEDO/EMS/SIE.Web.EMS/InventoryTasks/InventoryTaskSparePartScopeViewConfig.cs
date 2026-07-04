using SIE.Core.Equipments;
using SIE.EMS.InventoryTasks;
using SIE.Web.Common;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>
    /// 工治具盘点范围
    /// </summary>
    public class InventoryTaskSparePartScopeViewConfig : WebViewConfig<InventoryTaskSparePartScope>
    {

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            
            View.UseDetail(3);
            View.Property(p => p.WarehouseCode).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.StorageAreas).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.StorageLocations).ShowInDetail(columnSpan: 1).Readonly();

            View.Property(p => p.PartType).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ItemCategoryId).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.SparePartId).ShowInDetail(columnSpan: 1).Readonly();

            View.Property(p => p.ControlMethod).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.IsFixAsset).Readonly();
            View.Property(p => p.AssetsCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.ReloadDataOnPopping = true; }).Readonly();

            View.Property(p => p.AssetOwnerId).Readonly();
        }
    }
}
