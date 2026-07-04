using SIE.Common.Menus;
using System.Collections.Generic;

namespace SIE.Wpf.Andon.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class AndonMenuInit : IWpfMenuInit
    {
        const string mesCollection = "MES(CS).生产采集";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();
            
            res.Add(new MenuDto()
            {
                TreeKey = mesCollection,
                Label = "安灯管理",
                EntityType = typeof(Andon.AndonManageViewModel)
            });

            return res;
        }
    }
}
