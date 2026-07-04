using SIE.LES;
using SIE.LES.LesStockCounts;
using SIE.LES.LinesideWarehouses;
using SIE.LES.MaterialMoves;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.LES.MaterialReceives;
using SIE.LES.MaterialReturnApplys;
using SIE.LES.Reports;
using SIE.LES.StockOrders;
using SIE.LES.StockPlans;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.LES;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.LES
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
            CommonModel.Modules.AddModules(
                //new WebModuleMeta()
                //{
                //    Label = "备料单",
                //    EntityType = typeof(StockOrder),
                //}, 
                new WebModuleMeta()
                {
                    Label = "备料模式维护-拉式",
                    EntityType = typeof(PrepareItemPull),
                }, new WebModuleMeta()
                {
                    Label = "备料模式维护-推式",
                    EntityType = typeof(PrepareItemPush),
                }, new WebModuleMeta()
                {
                    Label = "产线线边仓维护".L10N(),
                    EntityType = typeof(LinesideWarehouse)
                },
                //new WebModuleMeta()
                //{
                //    Label = "备料计划".L10N(),
                //    EntityType = typeof(StockPlan)
                //},
                new WebModuleMeta()
                {
                    Label = "物料接收".L10N(),
                    EntityType = typeof(MaterialReceive)
                }, new WebModuleMeta()
                {
                    Label = "物料接收记录",
                    EntityType = typeof(MaterialReceiveRecord)
                }, new WebModuleMeta()
                {
                    Label = "线边仓盘点".L10N(),
                    EntityType = typeof(LesStockCount)
                },
                //new WebModuleMeta()
                //{
                //    Label = "备料单合并下发规则".L10N(),
                //    EntityType = typeof(StockOrderMergeIssued)
                //}, 
                new WebModuleMeta()
                {
                    Label = "备料需求单".L10N(),
                    EntityType = typeof(MaterialPreparation)
                }, new WebModuleMeta()
                {
                    Label = "工单需求汇总报表".L10N(),
                    EntityType = typeof(WoDemandReport)
                }, new WebModuleMeta()
                {
                    Label = "工单备料汇总".L10N(),
                    EntityType = typeof(WorkOrderMpViewModel)
                }, new WebModuleMeta()
                {
                    Label = "退料申请".L10N(),
                    EntityType = typeof(MaterialReturnApply)
                }, new WebModuleMeta()
                {
                    Label = "工单挪料记录".L10N(),
                    EntityType = typeof(MaterialMoveRecord)
                });
        }
    }
}
