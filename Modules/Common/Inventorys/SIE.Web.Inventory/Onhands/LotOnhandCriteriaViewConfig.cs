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
    internal class LotOnhandCriteriaViewConfig : WebViewConfig<LotOnhandCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Warehouse).UseAllWarehouseEditor(p => { p.SearchFieldList.Add(Warehouse.CodeProperty.Name); p.SearchFieldList.Add(Warehouse.NameProperty.Name); }).Show();
                View.Property(p => p.StorageArea).UseDataSource((e, p, s) =>
                {
                    var warehouse = (e as LotOnhandCriteria).Warehouse;
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageAreas(warehouse?.Id, s, p);
                }).UsePagingLookUpEditor(p => { p.SearchFieldList.Add(StorageArea.CodeProperty.Name); p.SearchFieldList.Add(StorageArea.NameProperty.Name); }).Show();

                View.Property(p => p.StorageLocation).UseDataSource((e, p, s) =>
                {
                    var warehouse = (e as LotOnhandCriteria).Warehouse;
                    var storageArea = (e as LotOnhandCriteria).StorageArea;
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(storageArea?.Id, warehouse?.Id, s, p);
                }).UsePagingLookUpEditor(p => { p.SearchFieldList.Add(StorageLocation.CodeProperty.Name); p.SearchFieldList.Add(StorageLocation.NameProperty.Name); }).Show();
                View.Property(p => p.Item).UseDataSource((e, p, s) =>
                {
                    var lotOnhandCriteria = e as LotOnhandCriteria;
                    if (lotOnhandCriteria == null)
                        return new EntityList<Item>();
                    return RT.Service.Resolve<ItemController>().GetItemDatas(p, s);
                }).UsePagingLookUpEditor().Show();
                View.Property(p => p.LotCode).Show();
                View.Property(p => p.StorerCode).Show();
                View.Property(p => p.ProjectNo).Show();
                View.Property(p => p.TaskNo).Show();
                View.Property(p => p.IsZero).Show();
            }
        }
    }
}
