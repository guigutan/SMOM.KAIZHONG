using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Stations;
using SIE.Tech.VictoryStandards;

namespace SIE.Web.Tech
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class TechMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            const string mesTech = "MES.工艺建模";
            res.Add(new MenuDto()
            {
                TreeKey = mesTech,
                Label = "工序",
                EntityType = typeof(Process)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTech,
                Label = "工位",
                EntityType = typeof(Station)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTech,
                Label = "胜制方案",
                EntityType = typeof(VictoryStandard)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTech,
                Label = "工序加工时长",
                EntityType = typeof(ProcessDuration)
            });
            res.Add(new MenuDto()
            {
                TreeKey = mesTech,
                Label = "工艺路线",
                EntityType = typeof(Routing)
            });


            return res;
        }

    }
}
