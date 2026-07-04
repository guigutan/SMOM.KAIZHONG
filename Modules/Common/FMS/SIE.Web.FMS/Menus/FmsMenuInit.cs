using SIE.Common.Menus;
using SIE.FMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.FMS.Menus
{
    /// <summary>
    /// FMS菜单初始化
    /// </summary>
    public class FmsMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>() {
                new MenuDto()
                {
                    TreeKey = "QMS",
                    IsLeafNode = false,
                    Label = "文件管理",
                },
                new MenuDto()
                {
                    TreeKey = "QMS.文件管理",
                    EntityType = typeof(SIE.FMS.FileManage),
                    Label = "文件管理",
                },
                new MenuDto()
                {
                    TreeKey = "QMS.文件管理",
                    EntityType = typeof(FileUserGroup),
                    Label = "文件用户组",
                },
            };
            return res;
        }
    }
}
