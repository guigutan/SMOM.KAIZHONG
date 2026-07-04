
using System;
using SIE.Modules;
using SIE.Web.ESop;
using SIE.MetaModel;
using SIE.ESop.Documents;
using SIE.ESop.Displays;
using SIE.ESop.EngDocuments;

[assembly: Module(typeof(Module))]
namespace SIE.Web.ESop
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
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "文档集".L10N(),
                EntityType = typeof(DocumentCollection),
            }); 
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    Label = "显示点".L10N(),
                    EntityType = typeof(DisplayPoint)
                });
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "工程文件维护".L10N(),
                EntityType = typeof(EngDocument),
            }); CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "工程文件使用类型".L10N(),
                EntityType = typeof(FileUseDetail),
            });
        }
    }
}
