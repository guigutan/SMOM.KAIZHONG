using SIE.MetaModel;
using SIE.Modules;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.Holidays;
using SIE.Resources.PersonnelSkills;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs;
using SIE.Resources.ProcessTechTypes;
using SIE.Resources.ShiftTypes;
using SIE.Resources.Skills;
using SIE.Resources.UserGroups;
using SIE.Resources.WipResources;
using SIE.Resources.WorkCenters;
using SIE.Web.Configs;
using SIE.Web.Resources.WipResources;
using System;
[assembly: Module(typeof(SIE.Web.Resources.Module))]

namespace SIE.Web.Resources
{
    /// <summary>
    /// 视图插件
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 重写插件初始化方法
        /// </summary>
        /// <param name="app">IApp</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            //FilterModuelResourceConfig.Add(GetType());
            WebResourceConfig.AddFilterReourceNames("SIE.Web.Resources.WipResources.Scripts.WipResourceLayout.js");
        }
        /// <summary>
        /// 程序模块初始化
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta
            {
                Label = "企业模型",
                EntityType = typeof(Enterprise)
            }, new WebModuleMeta
            {
                Label = "员工维护",
                EntityType = typeof(Employee)
            }, new WebModuleMeta
            {
                Label = "班组维护",
                EntityType = typeof(WorkGroup)
            }, new WebModuleMeta
            {
                Label = "班制",
                EntityType = typeof(ShiftType)
            }, new WebModuleMeta
            {
                Label = "日历方案",
                EntityType = typeof(CalendarScheme),
                //BlocksTemplate = typeof(CalendarSchemeTemplate)
            }, new WebModuleMeta
            {
                Label = "生产资源",
                EntityType = typeof(WipResource),
                BlocksTemplate = typeof(WipResourceTemplate)
            }, new WebModuleMeta
            {
                Label = "技能分类",
                EntityType = typeof(SkillCategory)
            }, new WebModuleMeta
            {
                Label = "技能清单",
                EntityType = typeof(Skill)
            }, new WebModuleMeta
            {
                Label = "法定假期",
                EntityType = typeof(Holiday),
            }, new WebModuleMeta
            {
                Label = "制程工艺类型",
                EntityType = typeof(ProcessTechType),
            }, new WebModuleMeta
            {
                Label = "制程工艺",
                EntityType = typeof(ProcessTech),
            }, new WebModuleMeta()
            {
                Label = "工段",
                EntityType = typeof(ProcessSegment),
            }
            , new WebModuleMeta()
            {
                Label = "工作中心",
                EntityType = typeof(WorkCenter)
            }, new WebModuleMeta()
            {
                Label = "人员技能基础数据",
                EntityType = typeof(PersonnelSkill)
            },new WebModuleMeta()
            { 
                EntityType = typeof(UserGroupLog),
                Label = "用户组操作日志"
            }
            );

            var user = CommonModel.Modules.FindModule(typeof(SIE.Rbac.Users.User));
            if (user != null)
                user.IsOnlyAdmin = false;
            var userGroup = CommonModel.Modules.FindModule(typeof(SIE.Rbac.Users.UserGroup));
            if (userGroup != null)
                userGroup.IsOnlyAdmin = false;
        }
    }
}
