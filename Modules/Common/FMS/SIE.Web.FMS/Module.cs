using SIE.FMS;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.FMS;
using System;

[assembly: Module(typeof(SIE.Web.FMS.Module))]
namespace SIE.Web.FMS
{
    /// <summary>
    /// UI模块
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
            CommonModel.Modules.AddModules(
                    new WebModuleMeta()
                    {
                        Label = "文件管理",
                        EntityType = typeof(SIE.FMS.FileManage),
                        BlocksTemplate = typeof(FileManageTemplate)
                    },
                    new WebModuleMeta()
                    {
                        Label = "文件用户组",
                        EntityType = typeof(FileUserGroup)
                    }
                   );
        }
    }
}
