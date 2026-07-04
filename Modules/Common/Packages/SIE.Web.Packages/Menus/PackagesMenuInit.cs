using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.Packages;
using SIE.Packages.Boxs;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.QrCodeParseRules;

namespace SIE.Web.Packages
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class PackagesMenuInit : IWebMenuInit
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
                TreeKey = "MES.物料管理",
                Label = "物料标签",
                EntityType = typeof(ItemLabel)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "SNest.基础功能",
                Label = "周转箱型号",
                EntityType = typeof(TurnoverBoxModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.基础功能",
                Label = "周转箱",
                EntityType = typeof(TurnoverBox)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "SNest.通用配置",
                Label = "包装单位",
                EntityType = typeof(PackingUnit)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.通用配置",
                Label = "包装规则设置",
                EntityType = typeof(PackageRule)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "WMS.基础资料",
                Label = "二维码解析规则",
                EntityType = typeof(QrCodeParseRule)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "WMS.基础资料",
                Label = "复核装箱规则",
                EntityType = typeof(RePackageRule)
            });
           
            return res;
        }

    }
}
