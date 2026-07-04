using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.OnLoans;
using SIE.MES.TeamManagement.RatedItems;
using SIE.MES.TeamManagement.ScoreRecords;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.MES.TeamManagement.SikllAuthentications;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.MES.TeamManagement.ClockingIns;
using SIE.Web.MES.TeamManagement.ShiftSchedules;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            AddPropertyEditor(app);
            WebResourceConfig.AddFilterModule(GetType());
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "评分记录".L10N(),
                EntityType = typeof(ScoreRecord)
            }, new WebModuleMeta
            {
                Label = "考勤机管理".L10N(),
                EntityType = typeof(ClockInMachine)
            }, new WebModuleMeta
            {
                Label = "排班表".L10N(),
                EntityType = typeof(ShiftSchedule),
                BlocksTemplate = typeof(ShiftScheduleTemplate)
            }, new WebModuleMeta
            {
                Label = "评分项目".L10N(),
                EntityType = typeof(RatedItem)
            }, new WebModuleMeta
            {
                Label = "借调明细表".L10N(),
                EntityType = typeof(WorkGroupOnLoan)
            }, new WebModuleMeta
            {
                Label = "员工技能认证管理".L10N(),
                EntityType = typeof(SkillAuthentication),
                UIGenerator = "SIE.Web.MES.TeamManagement.SikllAuthentications.SkillAuthUIGenerator"
            }, new WebModuleMeta
            {
                Label = "人员工时统计".L10N(),
                EntityType = typeof(EmployeeClockInAttent),
                ViewGroup = EmployeeClockInAttentViewConfig.AttentViewGroup
            });
        }

        /// <summary>
        /// 添加属性编辑器
        /// </summary>
        /// <param name="app">应用程序</param>
        private void AddPropertyEditor(IApp app)
        {
            //添加属性编辑器
        }
    }
}