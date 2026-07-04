using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Barcodes.WipBatchs;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Barcodes;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 条码模块类
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                Label = "条码打印".L10N(),
                EntityType = typeof(PrintWorkOrder),
                ViewGroup = BarcodePrintViewConfig.BarcodePrintView
            }, new WebModuleMeta()
            {
                Label = "条码报废".L10N(),
                EntityType = typeof(Barcode),
                ViewGroup = BarcodeViewConfig.ScrapView,
                BlocksTemplate = typeof(BarcodeScrapTemplate)
            }, new WebModuleMeta()
            {
                Label = "条码补打".L10N(),
                EntityType = typeof(BarcodeReprint),
                BlocksTemplate = typeof(BarcodeReprintTemplate)
            }, new WebModuleMeta()
            {
                Label = "条码打印日志".L10N(),
                EntityType = typeof(BarcodeLog)
            }
            //, new WebModuleMeta()
#pragma warning disable S125 // Sections of code should not be commented out
            //{
            //    Label = "条码领用",
            //    EntityType = typeof(BarcodeRange)
            //}
            , new WebModuleMeta()
            {
                Label = "批次生成".L10N(),
                EntityType = typeof(BatchWorkOrder),
                ViewGroup = WipBatchs.BatchWorkOrderViewConfig.BatchWorkOrderView
            }, new WebModuleMeta()
            {
                Label = "条码挂起".L10N(),
                EntityType = typeof(BarcodePending),
                BlocksTemplate = typeof(BarcodePendingTemplate)
            }
            //,new WebModuleMeta()
            //{
            //    EntityType = typeof(PanelWorkOrder),
            //    Label = "拼板码打印".L10N(),
            //    ViewGroup = "PanelPrintView"
            //}
            );
        }
    }
}