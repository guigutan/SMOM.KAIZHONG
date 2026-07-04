using SIE.DIST;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Runtime;
using SIE.Wpf.DIST;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    internal class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序对象</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations; //模块

            AddNewPropertyEditors(app); //属性编辑器

            //如果当前是在客户端，则需要尝试在启动时生成数据库。
            if (RT.Location.IsWPFUI)
            {
                (app as IClientApp).MainProcessStarting += OnMainProcessStarting;
            }
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">应用程序对象</param>
        private static void AddNewPropertyEditors(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
            };
        }

        /// <summary>
        /// 模块
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private static void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta
            {
                Label = "配送管理".L10N(),
                EntityType = typeof(GoodsIssue)
            },
            new WPFModuleMeta
            {
                Label = "配送单".L10N(),
                EntityType = typeof(DistributionBill)
            });
        }

        /// <summary>
        /// 主过程开始前事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private static void OnMainProcessStarting(object sender, EventArgs e)
        {
            //配置了以下配置项
        }
    }
}
