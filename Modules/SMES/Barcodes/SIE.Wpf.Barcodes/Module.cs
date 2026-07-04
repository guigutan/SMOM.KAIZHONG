using SIE.Barcodes;
using SIE.Barcodes.WipBatchs;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.Barcodes;
using SIE.Wpf.Barcodes.BatchBarcodes.Editors;
using SIE.Wpf.Barcodes.Editors;
using SIE.Wpf.Barcodes.UITemplates;
using SIE.Wpf.Barcodes.WipBatchs;
using SIE.Wpf.Barcodes.WipBatchs.Editors;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Wpf.Barcodes
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    internal class Module : UIModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += AddDevModules;
            AddNewPropertyEditor(app);
        }

        /// <summary>
        /// 模块
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private static void AddDevModules(object sender, EventArgs e)
        {
            if (!RT.Location.IsWebUI)
            {
                CommonModel.Modules.AddModules(new WPFModuleMeta()
                {
                    Label = "条码报废".L10N(),
                    EntityType = typeof(Barcode),
                    BlocksTemplate = typeof(BarcodeScrapUITemplate)
                }, new WPFModuleMeta()
                {
                    Label = "条码补打".L10N(),
                    EntityType = typeof(BarcodeReprint),
                    BlocksTemplate = typeof(BarcodeReprintUITemplate)
                }, new WPFModuleMeta()
                {
                    Label = "条码打印日志".L10N(),
                    EntityType = typeof(BarcodeLog)
                }, new WPFModuleMeta()
                {
                    Label = "条码领用".L10N(),
                    EntityType = typeof(BarcodeRange)
                }, new WPFModuleMeta()
                {
                    Label = "条码打印".L10N(),
                    EntityType = typeof(PrintWorkOrder),
                    BlocksTemplate = typeof(ListUITemplate),
                    ViewGroup = BarcodePrintViewConfig.BarcodePrintView
                }, new WPFModuleMeta()
                {
                    Label = "批次生成".L10N(),
                    EntityType = typeof(BatchWorkOrder),
                    BlocksTemplate = typeof(ListUITemplate),
                    ViewGroup = BatchWorkOrderViewConfig.BatchWorkOrderView
                });
            }
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        /// <param name="app">应用程序</param>
        private static void AddNewPropertyEditor(IApp app)
        {
            app.AllModulesIntialized += (o, e) =>
            {
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(NumberRuleBarcodeLookUpEditor.EditorName, typeof(NumberRuleBarcodeLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(PrintTemplateLookUpEditor.EditorName, typeof(PrintTemplateLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(ItemBatchRuleLookUpEditor.EditorName, typeof(ItemBatchRuleLookUpEditor));
                AutoUI.BlockUIFactory.PropertyEditorFactory.Set(BatchPrintTemplateLookUpEditor.EditorName, typeof(BatchPrintTemplateLookUpEditor));
            };
        }
    }
}