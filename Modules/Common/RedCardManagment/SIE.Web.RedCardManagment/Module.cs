using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.RedCardManagment;
using System;
using System.Collections.Generic;
using System.Text;
[assembly: Module(typeof(Module))]
namespace SIE.Web.RedCardManagment
{
    public class Module : UIModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            WebResourceConfig.AddFilterModule(GetType());
        }

        /// <summary>
        /// 配置菜单
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    Label = "红牌管理",
                    EntityType = typeof(SIE.RedCardManagment.RedCards.RedCard)
                },
                new WebModuleMeta()
                {
                    Label = "红牌申请",
                    EntityType = typeof(SIE.RedCardManagment.RedCardApplyBills.RedCardApplyBill)
                },
                new WebModuleMeta()
                {
                    Label = "红牌日志查看",
                    EntityType = typeof(SIE.RedCardManagment.RedCards.RedCardLog)
                }
                );
        }
    }
}
