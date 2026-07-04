using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Projects;

namespace SIE.Web.EMS.EarlierStage
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class EarlierStageMenuInit : IWebMenuInit
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
                TreeKey = "EDO",
                Label = "项目预算管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.项目预算管理",
                Label = "预算管理",
                EntityType = typeof(Budget)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.项目预算管理",
                Label = "预算变更",
                EntityType = typeof(BudgetChange)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.项目预算管理",
                Label = "项目管理",
                EntityType = typeof(Project)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.项目预算管理",
                Label = "项目事项",
                EntityType = typeof(ProjectKeyItem)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.项目预算管理",
                Label = "项目变更",
                EntityType = typeof(ProjectChange)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.项目预算管理",
                Label = "项目结项",
                EntityType = typeof(ProjectClose)
            });

            return res;
        }

    }
}
