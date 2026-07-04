using SIE.MetaModel;
using SIE.Modules;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.InspSettings;
using SIE.ProductIntfc.ProductInsps;
using SIE.ProductIntfc.ProductStorages;
using SIE.Wpf.ProductIntfc;
using SIE.Wpf.ProductIntfc.FirstInsps;
using SIE.Wpf.ProductIntfc.InspSettings.Editors;
using SIE.Wpf.ProductIntfc.ProductStorages;
using SIE.Wpf.ProductIntfc.ProductStorages.Editors;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.ProductIntfc
{
    /// <summary>
    /// 模块
    /// </summary>
    /// <seealso cref="SIE.Modules.UIModule" />
    public class Module : UIModule
    {
        /// <summary>
        /// 模块的初始化方法。
        /// 框架会在启动时根据启动级别顺序调用本方法。
        /// 方法有两个职责：
        /// 1.依赖注入。
        /// 2.注册 app 生命周期中事件，进行特定的初始化工作。
        /// </summary>
        /// <param name="app">应用程序对象。</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            AddPropertyEditor(app);
        }

        /// <summary>
        /// 模块操作
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void App_ModuleOperations(object sender, System.EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "报检参数",
                EntityType = typeof(InspParameter)
            }, new WPFModuleMeta()
            {
                Label = "成品报检",
                EntityType = typeof(ProductInsp),
                ViewGroup = FirstInspViewConfig.FirstInspView
            }, new WPFModuleMeta()
            {
                Label = "报检单查询",
                EntityType = typeof(InspLog)
            }, new WPFModuleMeta()
            {
                Label = "首件报检",
                EntityType = typeof(FirstInsp),
                BlocksTemplate = typeof(ListUITemplate),
                ViewGroup = FirstInspViewConfig.FirstInspView
            }, new WPFModuleMeta()
            {
                Label = "成品入库参数",
                EntityType = typeof(ProductStorageParam)
            }, new WPFModuleMeta()
            {
                Label = "成品入库",
                EntityType = typeof(StorageWorkOrder),
                BlocksTemplate = typeof(ListUITemplate),
                ViewGroup = StorageWorkOrderViewConfig.StorageWorkOrderView
            });
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">应用程序生成周期定义</param>
        private static void AddPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(StorageParamLookUpEditor.EditorName, typeof(StorageParamLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(InspParamLookUpEditor.EditorName, typeof(InspParamLookUpEditor));
            };
        }
    }
}
