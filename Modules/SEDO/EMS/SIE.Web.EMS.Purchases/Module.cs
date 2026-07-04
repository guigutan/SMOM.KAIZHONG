using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.EMS.Purchases.PaymentPlans;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.EMS.Purchases;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.EMS.Purchases
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
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    EntityType = typeof(PurchaseRequisition),
                    Label = "资产采购申请".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(PurchaseOrder),
                    Label = "资产采购订单".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(PaymentPlan),
                    Label = "付款计划".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(EquipmentReceive),
                    Label = "设备接收".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(EquipmentAcceptance),
                    Label = "设备开箱验收".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(EquipmentInbound),
                    Label = "设备入库".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(SparePartReceive),
                    Label = "备件接收".L10N()
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(SparePartAcceptance),
                    Label = "备件验收".L10N()
                },
                 new WebModuleMeta()
                 {
                     EntityType = typeof(FixtureReceive),
                     Label = "工治具接收".L10N()
                 },
                 new WebModuleMeta()
                 {
                     EntityType = typeof(EquipmentSetup),
                     Label = "安装调试".L10N()
                 },
                 new WebModuleMeta()
                 {
                     EntityType = typeof(FixtureAcceptance),
                     Label = "工治具验收".L10N()
                 }
            );
        }
    }
}
