using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.Items;

namespace SIE.Web.Items
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class ItemsMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            const string treeKeyStr = "SNest.物料基础数据";
            var res = new List<MenuDto>();
            res.Add(new MenuDto()
            {
                TreeKey = "SNest",
                Label = "物料基础数据",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKeyStr,
                EntityType = typeof(Unit),
                Label = "单位",
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKeyStr,
                EntityType = typeof(ItemCategory),
                Label = "分类",
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKeyStr,
                EntityType = typeof(ItemPropertyDefinition),
                Label = "物料属性定义",
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKeyStr,
                EntityType = typeof(ProductFamily),
                Label = "产品族",
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKeyStr,
                EntityType = typeof(ProductModel),
                Label = "产品机型",
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKeyStr,
                EntityType = typeof(Item),
                Label = "物料",
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKeyStr,
                EntityType = typeof(ProductBom),
                Label = "产品BOM",
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKeyStr,
                EntityType = typeof(UnitConvert),
                Label = "单位转换",
            });         
            return res;
        }

    }
}
