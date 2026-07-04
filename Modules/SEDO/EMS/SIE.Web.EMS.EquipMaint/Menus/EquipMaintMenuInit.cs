using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.Maintains.Records;

namespace SIE.Web.EMS.EquipMaint
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class EquipMaintMenuInit : IWebMenuInit
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
                Label = "设备保养计划维护",
                EntityType = typeof(MaintainPlanViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "设备保养记录",
                EntityType = typeof(MaintainRecord)
            });

            return res;
        }

    }
}
