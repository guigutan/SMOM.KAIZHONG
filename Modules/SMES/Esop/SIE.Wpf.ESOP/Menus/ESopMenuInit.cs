using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.ESop.Displays;
using SIE.ESop.Documents;
using SIE.Wpf.ESop.Displays;
using System;

namespace SIE.Wpf.ESop
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class ESopMenuInit : IWpfMenuInit
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
                TreeKey = "MES(CS)",
                Label = "基础配置",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS).基础配置",
                Label = "显示点",
                EntityType = typeof(DisplayPoint)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS).基础配置",
                Label = "文档集",
                EntityType = typeof(DocumentCollection)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES(CS).基础配置",
                Label = "ESOP",
                EntityType = typeof(ESopViewModel)
            });

            return res;
        }

    }
}
