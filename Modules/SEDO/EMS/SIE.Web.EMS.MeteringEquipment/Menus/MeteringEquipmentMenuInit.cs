using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;

namespace SIE.Web.EMS.MeteringEquipment
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class MeteringEquipmentMenuInit : IWebMenuInit
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
                Label = "计量设备台账",
                EntityType = typeof(MeteringEquipmentAccount)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "计量设备定检",
                EntityType = typeof(Calibration)
            });
            return res;
        }

    }
}
