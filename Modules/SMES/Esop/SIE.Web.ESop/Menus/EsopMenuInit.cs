using SIE.Common.Menus;
using SIE.ESop.Displays;
using SIE.ESop.Documents;
using SIE.ESop.EngDocuments;
using System;
using System.Collections.Generic;

namespace SIE.Web.ESop.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class EsopMenuInit : IWebMenuInit
    {
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "MES",
                Label = "ESOP",
                IsLeafNode = false,
            });

            const string esopMenu = "MES.ESOP";

            res.Add(new MenuDto()
            {
                TreeKey = esopMenu,
                Label = "文档集".L10N(),
                EntityType = typeof(DocumentCollection),
            });

            res.Add(new MenuDto()
            {
                TreeKey = esopMenu,
                Label = "显示点".L10N(),
                EntityType = typeof(DisplayPoint)
            });

            res.Add(new MenuDto()
            {
                TreeKey = esopMenu,
                Label = "工程文件维护".L10N(),
                EntityType = typeof(EngDocument)
            });
            res.Add(new MenuDto()
            {
                TreeKey = esopMenu,
                Label = "工程文件使用类型".L10N(),
                EntityType = typeof(FileUseDetail)
            });
            return res;
        }
    }
}
