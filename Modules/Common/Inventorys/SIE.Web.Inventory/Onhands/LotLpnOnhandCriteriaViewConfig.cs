using SIE.Domain;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Web.Inventory.Onhands
{
    /// <summary>
    /// 库位库存查询 视图配置
    /// </summary>
    internal class LotLpnOnhandCriteriaViewConfig : WebViewConfig<LpnOnhandCriteria>
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
                    var warehouse = (e as LpnOnhandCriteria).WarehouseCode;
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageAreasByWhCode(warehouse, s, p);
                }).UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(StorageArea.CodeProperty.Name);
                    p.SearchFieldList.Add(StorageArea.NameProperty.Name);
                }).Show(ShowInWhere.Hide);

                View.Property(p => p.StorageLocation).UseDataSource((e, p, s) =>
                {
                    var warehouse = (e as LpnOnhandCriteria).Warehouse;
                    var storageArea = (e as LpnOnhandCriteria).StorageArea;
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(storageArea?.Id, warehouse?.Id, s, p);
                }).UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(StorageLocation.CodeProperty.Name);
                    p.SearchFieldList.Add(StorageLocation.NameProperty.Name);
                }).Show(ShowInWhere.Hide);
                View.Property(p => p.ItemId).UseDataSource((e, p, s) =>
                {
                    var lpnOnhandCriteria = e as LpnOnhandCriteria;
                    if (lpnOnhandCriteria == null)
                        return new EntityList<Item>();
                    return RT.Service.Resolve<ItemController>().GetItemDatas(p, s);
                }).UsePagingLookUpEditor().Show(ShowInWhere.Hide);
                View.Property(p => p.StorageAreaCode).Show();
                View.Property(p => p.StorageLocationCode).Show();
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
                View.Property(p => p.LotCode).Show();
                View.Property(p => p.Lpn).Show();                
                View.Property(p => p.State).Show();
                View.Property(p => p.StorerCode).Show();
                View.Property(p => p.ProjectNo).Show();
                View.Property(p => p.TaskNo).Show();
                View.Property(p => p.IsZero).Show().Visibility(p => !p.HideZero);
            }
        }
    }
}
