using SIE.Common.Menus;
using SIE.ShipPlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.ShipPlan.Menus
{
    internal class ShipPlanMenuInit : IWebMenuInit
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
                TreeKey = "WMS.基础资料",
                EntityType = typeof(AssignWarehouseRule),
                Label = "分配仓库规则".L10N(),
            });
            res.Add(new MenuDto()
            {
                TreeKey = "WMS.基础资料",
                EntityType = typeof(DeliveryPlan),
                Label = "发货计划".L10N(),
            });

            return res;
        }

    }     
}
