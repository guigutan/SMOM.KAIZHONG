using SIE.Common.Menus;
using SIE.Warehouses;
using SIE.Warehouses.Stations;
using System.Collections.Generic;

namespace SIE.Web.Warehouses.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class WarehouseMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            const string treeKey = "WMS.基础资料";
            var res = new List<MenuDto>();
            res.Add(new MenuDto()
            {
                Label = "WMS",
                Sort = 0,
                Icon = "wms icon-wms",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "WMS",
                Label = "基础资料",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "仓库",
                EntityType = typeof(Warehouse)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "库区",
                EntityType = typeof(StorageArea)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "库位",
                EntityType = typeof(StorageLocation)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "工作区",
                EntityType = typeof(WorkArea)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "巷道",
                EntityType = typeof(Routeway)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "站台",
                EntityType = typeof(Station)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "站台组",
                EntityType = typeof(StationGroup)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "LED屏幕基础数据",
                EntityType = typeof(LED)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "LED屏幕显示的风格样式",
                EntityType = typeof(LEDShowStyle)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "ERP子库",
                EntityType = typeof(ErpWarehouse),
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "逻辑分区",
                EntityType = typeof(LogicArea)              
            });
            return res;
        }

    }
}
