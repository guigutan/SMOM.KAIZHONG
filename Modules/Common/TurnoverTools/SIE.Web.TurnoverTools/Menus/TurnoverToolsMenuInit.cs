using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.TurnoverTools.TurnoverTools;

namespace SIE.Web.TurnoverTools
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class TurnoverToolsMenuInit : IWebMenuInit
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
                EntityType = typeof(TurnoverToolModel),
                Label = "周转工具型号维护",
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.基础功能",
                EntityType = typeof(TurnoverTool),
                Label = "周转工具",
            });
            return res;
        }

    }
}
