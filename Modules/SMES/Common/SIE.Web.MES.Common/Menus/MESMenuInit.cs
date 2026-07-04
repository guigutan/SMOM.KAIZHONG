using SIE.Common.Menus;
using SIE.MES.Common.HomeMenusConfigs;
using System.Collections.Generic;

namespace SIE.Web.MES.Common
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class MESMenuInit : IWebMenuInit
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
                TreeKey = "MES",
                Label = "生产通用管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.生产通用管理",
                Label = "触摸屏首页设置",
                EntityType = typeof(HomeMenusConfig)
            });

            return res;
        }
    }
}
