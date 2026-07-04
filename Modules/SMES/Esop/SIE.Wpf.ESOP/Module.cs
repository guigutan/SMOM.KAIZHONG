using SIE.ESop.Displays;
using SIE.ESop.Documents;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Modules;
using SIE.Wpf.ESop;
using SIE.Wpf.ESop.Displays.Command;
using SIE.Wpf.ESop.Editors;
using SIE.Wpf.MES.WIP;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.ESop
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序对象</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations; //模块;
            AddNewPropertyEditors(app);

            if (RT.Location.IsWPFUI)
            {
                EntityViewMetaFactory.MetaCreated += (s, e) =>
                {
                    if (typeof(DataCollectionViewModel).IsAssignableFrom(e.Meta.EntityType))
                    {
                        e.Meta.AsWPFView().UseCommands(typeof(PlayESopCommand));
                    }
                };
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
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(DocumentSelectEditor.EditorNameEX, typeof(DocumentSelectEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ESopEnterpriseLookUpEditor.EditorName, typeof(ESopEnterpriseLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(DisplayPointLookUpEditor.EditorName, typeof(DisplayPointLookUpEditor));
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
                Label = "显示点".L10N(),
                EntityType = typeof(DisplayPoint)
            }, new WPFModuleMeta
            {
                Label = "文档集".L10N(),
                EntityType = typeof(DocumentCollection)
            });
        }
    }
}