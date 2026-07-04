using SIE.Common.Menus;
using SIE.Recheck.Common.ItemRecheck;
using System.Collections.Generic;

namespace SIE.Web.Recheck
{
    internal class RecheckMenuInit : IWebMenuInit
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
                TreeKey = "SNest.物料基础数据",
                EntityType = typeof(ItemRecheckProgram),
                Label = "物料复检方案",
            });
            

            return res;
        }

    }     
}
