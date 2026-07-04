using SIE.Domain;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.Warehouses;
using SIE.Web.Warehouses;

namespace SIE.Web.Inventory.Onhands
{
    /// <summary>
    /// 库位库存查询 视图配置
    /// </summary>
    internal class LocationOnhandCriteriaViewConfig : WebViewConfig<LocationOnhandCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseCode).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Warehouse).FullName;
                    p.XType = "InvMultiWhComboPopup";
                    p.LinkField = Warehouse.CodeProperty.Name;
                    p.ValueField = Warehouse.CodeProperty.Name;
                    p.DisplayField = Warehouse.CodeProperty.Name;
                    p.Editable = true;
                    p.Separator = ",";
                }).Show()
                .UseListSetting(f => f.HelpInfo = "查多个用英文逗号分隔");
                View.Property(p => p.StorageArea).UseDataSource((e, p, s) =>
                {
                    var warehouse = (e as LocationOnhandCriteria).WarehouseCode;
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageAreasByWhCode(warehouse, s, p);
                }).UsePagingLookUpEditor(p => { p.SearchFieldList.Add(StorageArea.CodeProperty.Name); p.SearchFieldList.Add(StorageArea.NameProperty.Name); }).Show();

                View.Property(p => p.StorageLocation).UseDataSource((e, p, s) =>
                {
                    var warehouse = (e as LocationOnhandCriteria).WarehouseCode;
                    var storageArea = (e as LocationOnhandCriteria).StorageArea;
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationsByWhCode(storageArea?.Id, warehouse, s, p);
                }).UsePagingLookUpEditor(p => { p.SearchFieldList.Add(StorageLocation.CodeProperty.Name); p.SearchFieldList.Add(StorageLocation.NameProperty.Name); }).Show();
                View.Property(p => p.ItemCode).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Item).FullName;
                    p.LinkField = Item.CodeProperty.Name;
                    p.ValueField = Item.CodeProperty.Name;
                    p.DisplayField = Item.CodeProperty.Name;
                    p.XType = "MultiItemComboPopup";
                    p.Editable = true;
                    p.Separator = ",";
                }).Show().UseListSetting(f => f.HelpInfo = "查多个用英文逗号分隔");
                View.Property(p => p.StorerCode).Show();
                View.Property(p => p.ProjectNo).Show();
                View.Property(p => p.TaskNo).Show();
                View.Property(p => p.IsZero).Show();
            }
        }
    }
}
