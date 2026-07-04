using SIE.Common.Menus;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockQueues;
using SIE.Dock.DockRunMts;
using SIE.Dock.YardMaintains;
using SIE.Dock.YardZones;
using System.Collections.Generic;

namespace SIE.Web.Dock.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class DockMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();
            const string parName = "WMS.基础资料";
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "月台维护",
                EntityType = typeof(DockMaintain),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "月台运行维护",
                EntityType = typeof(DockRunMt),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "园片区维护",
                EntityType = typeof(YardZone),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "月台预约",
                EntityType = typeof(DockAppoint),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "月台排队",
                EntityType = typeof(DockQueue),
            });
            res.Add(new MenuDto()
            {
                TreeKey = parName,
                Label = "园区维护",
                EntityType = typeof(YardMaintain),
            });            
            return res;
        }

    }
}
