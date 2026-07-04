using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.ERPInterface.Smom.InventoryControl;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.ERPInterface;
using SIE.Web.ERPInterface.Logs;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.ERPInterface
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
            app.MetaCompiled += App_MetaCompiled;
        }

        private void App_MetaCompiled(object sender, EventArgs e)
        {
            Page82Config.Add(typeof(DownloadExcReportViewModel).FullName, Page82.Page2);
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                EntityType = typeof(DownloadJobLog),
                Label = "下载任务记录".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(DownloadJobTime),
                Label = "接口任务时间戳".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(UploadTransaction),
                Label = "事务上传".L10N()
            },
            //new WebModuleMeta()
            //{
            //    EntityType = typeof(UploadTransactionLog),
            //    Label = "事务上传记录".L10N()
            //},
            new WebModuleMeta()
            {
                EntityType = typeof(ErpUploadLog),
                Label = "事务上传记录".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(UploadTransactionRule),
                Label = "交易上传规则".L10N()
            },
            new WebModuleMeta()
            {
                Label = "接口下载异常报表".L10N(),
                EntityType = typeof(DownloadExcReportViewModel),
                BlocksTemplate = typeof(DownloadExcReportTemplate)
            }
            );

            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                EntityType = typeof(ItemCategoryInf),
                Label = "分类中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(ItemInf),
                Label = "物料中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(EnterpriseInf),
                Label = "企业模型中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(EmployeeInf),
                Label = "员工中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(CustomerInf),
                Label = "客户中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(CustomerAddressInf),
                Label = "客户地址中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(SupplierInf),
                Label = "供应商中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(SupplierAddressInf),
                Label = "供应商地址中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(WarehouseInf),
                Label = "仓库中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(StorageAreaInf),
                Label = "库区中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(StorageLocationInf),
                Label = "库位中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(ProductBomInf),
                Label = "产品BOM中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(ProductBomDetailInf),
                Label = "产品BOM明细中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(WorkOrderInf),
                Label = "工单中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(WorkOrderBomInf),
                Label = "工单BOM中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(PurchaseOrderInf),
                Label = "PO中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(PurchaseOrderDetailInf),
                Label = "PO明细中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(AsnInf),
                Label = "ASN中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(AsnDetailInf),
                Label = "ASN明细中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(ShippingOrderInf),
                Label = "发运单中间表".L10N()
            },
            new WebModuleMeta()
            {
                EntityType = typeof(ShippingOrderDetailInf),
                Label = "发运单明细中间表".L10N()
            },
            //new WebModuleMeta()
            //{
            //    EntityType = typeof(GenericDispositionInf),
            //    Label = "账户别名中间表"
            //},
            new WebModuleMeta()
            {
                EntityType = typeof(ProductOrderBomInf),
                Label = "生产订单BOM中间表".L10N()
            },
            new WebModuleMeta()
            {
                Label = "库存对照表".L10N(),
                EntityType = typeof(InventoryControlViewModel),
                UIGenerator = "SIE.Web.ERPInterface.InventoryControl.InventoryControlGenerator"
            },
            new WebModuleMeta()
            {
                EntityType = typeof(ErpJobCloseRule),
                Label = "交易期关闭日".L10N()
            });

        }
    }
}