using SIE.MetaModel;
using SIE.Modules;
using SIE.Tech.OEE;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Stations;
using SIE.Tech.VictoryStandards;
using SIE.Web.Tech;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.Tech
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">程序</param>
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
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "工序",
                EntityType = typeof(Process),
                UIGenerator = "SIE.Web.Tech.Processs.ProcessAuthUIGenerator"
            }, new WebModuleMeta()
            {
                Label = "工位",
                EntityType = typeof(Station),
            }, new WebModuleMeta()
            {
                Label = "工艺路线",
                EntityType = typeof(Routing),
                BlocksTemplate = typeof(RoutingTemplate),
            }, new WebModuleMeta()
            {
                Label = "胜制方案",
                EntityType = typeof(VictoryStandard)
            }, new WebModuleMeta()
            {
                Label = "工序加工时长",
                EntityType = typeof(ProcessDuration)
            },new WebModuleMeta()
            { 
                EntityType = typeof(OeeProcess),
                Label = "OEE报表数采工序配置"
            });
        }
    }
}