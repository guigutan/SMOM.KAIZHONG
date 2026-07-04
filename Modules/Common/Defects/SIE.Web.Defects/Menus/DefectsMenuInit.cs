using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.Defects;
using SIE.Defects.InspectionItems;
using SIE.Defects.Measures;

namespace SIE.Web.Defects
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class DefectsMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();
            const string snestBaseFunction = "SNest.基础功能";

            res.Add(new MenuDto()
            {
                TreeKey = snestBaseFunction,
                EntityType = typeof(Defect),
                Label = "缺陷代码",
            });
            res.Add(new MenuDto()
            {
                TreeKey = snestBaseFunction,
                EntityType = typeof(DefectResponsibility),
                Label = "缺陷责任",
            });
            res.Add(new MenuDto()
            {
                TreeKey = snestBaseFunction,
                EntityType = typeof(RepairMeasure),
                Label = "维修措施",
            });
            res.Add(new MenuDto()
            {
                TreeKey = snestBaseFunction,
                EntityType = typeof(DefectGrade),
                Label = "缺陷等级",
            });

            res.Add(new MenuDto()
            {
                TreeKey = "QMS.基础数据",
                EntityType = typeof(InspectionMode),
                Label = "检验方式",
            });
            return res;
        }

    }
}
