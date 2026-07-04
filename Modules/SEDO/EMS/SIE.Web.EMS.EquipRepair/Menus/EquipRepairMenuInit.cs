using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.ExperienceDepots;
using SIE.EMS.EquipRepair.PlanRepairs;

namespace SIE.Web.EMS.EquipRepair
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class EquipRepairMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "维修经验库",
                EntityType = typeof(ExperienceDepot)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "维修管理",
                EntityType = typeof(EquipRepairBill)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "计划维修",
                EntityType = typeof(PlanRepair)
            });

            return res;
        }

    }
}
