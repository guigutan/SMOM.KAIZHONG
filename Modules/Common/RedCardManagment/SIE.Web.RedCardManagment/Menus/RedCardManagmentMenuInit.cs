using SIE.Common.Menus;
using SIE.RedCardManagment.RedCardApplyBills;
using SIE.RedCardManagment.RedCards;
using System.Collections.Generic;

namespace SIE.Web.RedCardManagment.Menus
{
    /// <summary>
    /// 红牌菜单初始化
    /// </summary>
    public class RedCardManagmentMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            const string strMenuInfo = "SNest.红牌管理";
            var res = new List<MenuDto>() {
                 new MenuDto()
                {
                    TreeKey = "SNest",
                    IsLeafNode = false,
                    Label = "红牌管理",
                },
                new MenuDto()
                {
                    TreeKey = strMenuInfo,
                    EntityType = typeof(RedCard),
                    Label = "红牌管理",
                },
                new MenuDto()
                {
                    TreeKey = strMenuInfo,
                    EntityType = typeof(RedCardApplyBill),
                    Label = "红牌申请",
                },
                new MenuDto()
                {
                    TreeKey = strMenuInfo,
                    EntityType = typeof(RedCardLog),
                    Label = "红牌日志查看",
                }
            };
            return res;
        }
    }
}
