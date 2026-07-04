using SIE.Core.ApiLogs;
using SIE.Core.Prints;
using SIE.Core.QmsStaticConst;
using SIE.Core.UserAgreements;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Core;
using SIE.Web.Core.UserAgreements.Templates;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Core
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
            // 添加模块定义
            CommonModel.Modules.AddModules(
                    new WebModuleMeta()
                    {
                        Label = "用户协议管理".L10N(),
                        EntityType = typeof(UserAgreement),
                        BlocksTemplate = typeof(UserAgreementTemplate),
                    },
                    new WebModuleMeta
                    {
                        Label = "统计系数表",
                        EntityType = typeof(StaticConst)
                    },
                     new WebModuleMeta
                     {
                         Label = "接口日志",
                         EntityType = typeof(ApiLog)
                     },
                     new WebModuleMeta()
                     {
                         Label = "PDA标签打印日志",
                         EntityType = typeof(PrintLog),
                     },
                     new WebModuleMeta()
                     {
                         Label = "KZ打印模板设置",
                         EntityType = typeof(PrinterSettingTpl),
                     }
            );
        }  
    }
}