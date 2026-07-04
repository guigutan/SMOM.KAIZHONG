using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.NavMenus
{
    /// <summary>
    /// 向导菜单控制器
    /// </summary>
    public partial class NavMenuController  
    {
        /// <summary>
        /// 获取向导菜单配置信息
        /// </summary>
        /// <returns></returns>
        public virtual List<NavMenuConfig> GetNavMenuConfigs()
        {
            var configs = new List<NavMenuConfig>();
            var navMenuDtos = new List<NavMenuDto>();
            foreach (var plugin in RT.GetAllModules())
            {
                if (plugin.Assembly == typeof(Entity).Assembly) 
                {
                    continue;
                }

                foreach (var type in plugin.Assembly.GetTypes())
                {
                    if (typeof(IWebNavMenuInit).IsAssignableFrom(type) && !type.IsAbstract) 
                    {
                        var webNavMenuInit = Activator.CreateInstance(type) as INavMenuInit;
                        navMenuDtos.AddRange(webNavMenuInit.GetNavMenuDtos());
                    }
                }
            }

            var rootNavMenuDtos = navMenuDtos.Where(p=>p.TreeKey.IsNullOrEmpty());
            foreach (var navMenuDto in rootNavMenuDtos)
            {
                string key = Guid.NewGuid().ToString();
                configs.Add(new NavMenuConfig()
                {
                    key = key,
                    desc = navMenuDto.Label,
                    type = navMenuDto.EntityType.GetQualifiedName()
                });
                CreateNavMenuConfigs(navMenuDto.Label, key, navMenuDtos, configs);
            }

            return configs;
        }

        /// <summary>
        /// 创建向导菜单配置信息
        /// </summary>
        /// <returns></returns>
        private void CreateNavMenuConfigs(string navMenulabel, string parentKey,IList<NavMenuDto> navMenuDtos, IList<NavMenuConfig> configs)
        {
            var subNavMenuDtos = navMenuDtos.Where(p => p.TreeKey == navMenulabel);
            foreach (var subNavMenuDto in subNavMenuDtos)
            {
                string key = Guid.NewGuid().ToString();
                configs.Add(new NavMenuConfig() { 
                    key = key,
                    desc = subNavMenuDto.Label,
                    parent = parentKey,
                    type = subNavMenuDto.EntityType.GetQualifiedName()
                });
                CreateNavMenuConfigs(subNavMenuDto.Label, key, navMenuDtos, configs);
            }
        }
    }
}
