using SIE.Dock.DockAppoints;
using SIE.Dock.DockGantt;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockQueues;
using SIE.Dock.DockRunMts;
using SIE.Dock.YardMaintains;
using SIE.Dock.YardZones;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Dock;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Dock
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
        }

        /// <summary>
        /// 配置菜单
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    Label = "月台维护",
                    EntityType = typeof(DockMaintain),
                },
                new WebModuleMeta()
                {
                    Label = "月台运行维护",
                    EntityType = typeof(DockRunMt),
                },
                new WebModuleMeta()
                {
                    Label = "园片区维护",
                    EntityType = typeof(YardZone),
                },
                new WebModuleMeta()
                {
                    Label = "月台预约",
                    EntityType = typeof(DockAppoint),
                },
                new WebModuleMeta()
                {
                    Label = "月台排队",
                    EntityType = typeof(DockQueue),
                },
                new WebModuleMeta()
                {
                    Label = "园区维护",
                    EntityType = typeof(YardMaintain),
                },
                new WebModuleMeta()
                {
                    Label = "月台地图表",
                    EntityType = typeof(DockGantt),
                    Url = "/Dock/LoadGantt/Load"
                }
                );
        }
    }
}