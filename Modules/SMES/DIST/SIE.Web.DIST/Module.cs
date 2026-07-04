using SIE.DIST;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.DIST;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.DIST
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
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            //CommonModel.Modules.AddModules(new WebModuleMeta()
            //{
            //    Label = "配送管理".L10N(),
            //    EntityType = typeof(GoodsIssue)
            //}, new WebModuleMeta()
            //{
            //    Label = "配送单".L10N(),
            //    EntityType = typeof(DistributionBill)
            //});
        }
    }
}