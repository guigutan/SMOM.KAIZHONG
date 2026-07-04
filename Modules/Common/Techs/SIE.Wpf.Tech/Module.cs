using SIE.MetaModel;
using SIE.Modules;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Stations;
using SIE.Wpf.Tech;
using SIE.Wpf.Tech.Routings;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.Tech
{
    /// <summary>
    /// 工艺模块初始化器
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 菜单设置
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "工位".L10N(),
                EntityType = typeof(Station)
            }, new WPFModuleMeta()
            {
                Label = "工序".L10N(),
                EntityType = typeof(Process)
            }, new WPFModuleMeta()
            {
                Label = "关键物料".L10N(),
                EntityType = typeof(KeyItem)
            }, new WPFModuleMeta()
            {
                Label = "工艺路线".L10N(),
                EntityType = typeof(Routing),
                CustomUI = typeof(RoutingDesign)
            });
        }
    }
}
