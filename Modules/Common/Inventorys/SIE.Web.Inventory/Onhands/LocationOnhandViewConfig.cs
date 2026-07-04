using SIE.Inventory.Onhands;
using SIE.MetaModel.View;
using SIE.Warehouses;

namespace SIE.Web.Inventory.Onhands
{
    /// <summary>
    /// 库位库存视图配置
    /// </summary>
    internal class LocationOnhandViewConfig : WebViewConfig<LocationOnhand>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.AddBehavior("SIE.Web.Inventory.LocationOnhandBehavior");
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.ItemCode).FixColumn().ShowInList(150);
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.StorageLocationCode);
            View.Property(p => p.ItemName).ShowInList(150);
            View.Property(p => p.ItemExtPropName).ShowInList(120);
            View.Property(p => p.Qty);
            View.Property(p => p.AvailableQty);
            View.Property(p => p.AllottedQty);
            View.Property(p => p.FreezingQty);
            View.Property(p => p.UnitName);
            View.Property(p => p.WarehouseName);
            View.Property(p => p.StorageLocationName);
            View.Property(p => p.StorerCode);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.TaskNo);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.ItemCode).ShowInList(150);
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.WarehouseName);
            View.Property(p => p.StorageLocationCode);
            View.Property(p => p.StorageLocationName);
            View.Property(p => p.Qty);
            View.Property(p => p.AvailableQty);
            View.Property(p => p.FreezingQty);
            View.Property(p => p.AllottedQty);
            View.Property(p => p.StorerCode);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.TaskNo);
            View.Property(p => p.ItemExtPropName).ShowInList(120);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Item).HasLabel("物料");
            View.Property(p => p.Warehouse).HasLabel("仓库");
            View.Property(p => p.StorageLocation).UsePagingLookUpEditor(p =>
            {
                p.SearchFieldList.Add(StorageLocation.CodeProperty.Name);
                p.SearchFieldList.Add(StorageLocation.NameProperty.Name);
            }).HasLabel("库位");
            View.Property(p => p.StorerCode);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.TaskNo);
        }
    }
}
