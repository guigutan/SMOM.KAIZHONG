using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Projects;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.EMS.EarlierStage;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.EMS.EarlierStage
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
            if (app != null)
            {
                app.ModuleOperations += App_ModuleOperations;
            }
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    EntityType = typeof(Budget),
                    Label = "预算管理".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(BudgetChange),
                    Label = "预算变更".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(Project),
                    Label = "项目管理".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(ProjectKeyItem),
                    Label = "项目事项".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(ProjectChange),
                    Label = "项目变更".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(ProjectClose),
                    Label = "项目结项".L10N()
                }
            );
        }
    }
}
