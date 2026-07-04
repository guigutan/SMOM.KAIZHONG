using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.DIST;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class DISTMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            //res.Add(new MenuDto()
            //{
            //    TreeKey = "MES",
            //    Label = "物料管理",
            //    IsLeafNode = false,
            //});
            //res.Add(new MenuDto()
            //{
            //    TreeKey = "MES.物料管理",
            //    Label = "配送管理",
            //    EntityType = typeof(GoodsIssue)
            //});
            //res.Add(new MenuDto()
            //{
            //    TreeKey = "MES.物料管理",
            //    Label = "配送单",
            //    EntityType = typeof(DistributionBill)
            //});

            return res;
        }

    }
}
