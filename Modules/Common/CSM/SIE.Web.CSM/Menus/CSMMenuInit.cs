using SIE.Common.Menus;
using SIE.CSM.Customers;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.CSM.Suppliers;
using System.Collections.Generic;

namespace SIE.Web.CSM.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class CSMMenuInit : IWebMenuInit
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
                TreeKey = "SNest.基础功能",
                EntityType = typeof(Supplier),
                Label = "供应商",
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.基础功能",
                EntityType = typeof(Customer),
                Label = "客户",
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.基础功能",
                EntityType = typeof(ItemInspCharacteristics),
                Label = "物料检验特性",
            });
            return res;
        }
    }
}
