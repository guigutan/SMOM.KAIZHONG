using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;

namespace SIE.Web.EMS.SpecialEquipment
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class SpecialEquipmentMenuInit : IWebMenuInit
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
                TreeKey = "EDO.设备管理",
                Label = "特种设备台账",
                EntityType = typeof(SpecialEquipmentAccount)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "特种设备定检",
                EntityType = typeof(RegularInspection)
            });

            return res;
        }

    }
}
