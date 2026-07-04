using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.Holidays;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs;
using SIE.Resources.ProcessTechTypes;
using SIE.Resources.ShiftTypes;
using SIE.Resources.Skills;
using SIE.Resources.WipResources;

namespace SIE.Web.Resources
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class ResourcesMenuInit : IWebMenuInit
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
                TreeKey = "MES.工艺建模",
                Label = "工段",
                EntityType = typeof(ProcessSegment)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "SNest.基础功能",
                Label = "班组维护",
                EntityType = typeof(WorkGroup)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "SNest.基础功能",
                Label = "员工维护",
                EntityType = typeof(Employee)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "SNest",
                Label = "工厂建模",
                IsLeafNode = false,
            });

            GetFactoryMenuDto(res);

            res.Add(new MenuDto()
            {
                TreeKey = "MES.技能管理",
                Label = "技能分类",
                EntityType = typeof(SkillCategory)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "MES.技能管理",
                Label = "技能清单",
                EntityType = typeof(Skill)
            });

            res.Add(new MenuDto()
            {
                Label = "APS",
                Sort = 0,
                Icon = "aps icon-aps",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "APS",
                Label = "生产资料",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "APS.生产资料",
                Label = "制程工艺类型",
                EntityType = typeof(ProcessTechType)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "APS.生产资料",
                Label = "制程工艺",
                EntityType = typeof(ProcessTech)
            });

            return res;
        }

        /// <summary>
        /// 获取工厂建模
        /// </summary>
        /// <param name="res"></param>
        private void GetFactoryMenuDto(List<MenuDto> res)
        {
            const string treeKey = "SNest.工厂建模";
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "企业模型",
                EntityType = typeof(Enterprise)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "班制",
                EntityType = typeof(ShiftType)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "日历方案",
                EntityType = typeof(CalendarScheme)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "法定假期",
                EntityType = typeof(Holiday)
            });
            res.Add(new MenuDto()
            {
                TreeKey = treeKey,
                Label = "生产资源",
                EntityType = typeof(WipResource)
            });
        }
    }
}
