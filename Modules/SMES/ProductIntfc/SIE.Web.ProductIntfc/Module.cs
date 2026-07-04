using SIE.MetaModel;
using SIE.Modules;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspSettings;
using SIE.ProductIntfc.OutputProducts;
using SIE.ProductIntfc.ProductInsps;
using SIE.ProductIntfc.ProductStorages;
using SIE.Web.ProductIntfc;
using SIE.Web.ProductIntfc.FirstInsps;
using SIE.Web.ProductIntfc.OutputProducts;
using SIE.Web.ProductIntfc.ProductInsps;
using SIE.Web.ProductIntfc.ProductStorages;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.ProductIntfc
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
            CommonModel.Modules.AddModules(new WebModuleMeta
            {
                Label = "报检参数".L10N(),
                EntityType = typeof(InspParameter)
            }, new WebModuleMeta()
            {
                Label = "成品报检".L10N(),
                EntityType = typeof(ProductInsp),
                ViewGroup = ProductInspViewConfig.ProductInspViewGroup
            }, new WebModuleMeta()
            {
                Label = "首件报检".L10N(),
                EntityType = typeof(FirstInsp),
                ViewGroup = FirstInspViewConfig.FirstInspView,
            }, new WebModuleMeta()
            {
                Label = "成品入库参数".L10N(),
                EntityType = typeof(ProductStorageParam)
            }, new WebModuleMeta()
            {
                Label = "成品入库".L10N(),
                EntityType = typeof(StorageWorkOrder),
                ViewGroup = StorageWorkOrderViewConfig.StorageWorkOrderView,
                UIGenerator = "SIE.Web.ProductIntfc.ProductStorages.Scripts.CheckBoxUIGenerator"
            }
            , new WebModuleMeta()
            {
                Label = "联/副产品入库".L10N(),
                EntityType = typeof(OutputProduct),
                ViewGroup = OutputProductViewConfig.OutputProductView,
                
            }
            );

            
        }
    }
}