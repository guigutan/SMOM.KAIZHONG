using SIE.Inventory.Onhands;
using SIE.Inventory.Strategy;
using SIE.Inventory.Task;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Inventory;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.Inventory
{
    /// <summary>
    /// UIModule
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
                        Label = "上架规则",
                        EntityType = typeof(OnShelvesRule),
                    },
                    new WebModuleMeta()
                    {
                        Label = "批次和LPN库存",
                        EntityType = typeof(LotLpnOnhand),
                    },
                    new WebModuleMeta()
                    {
                        Label = "批次库存",
                        EntityType = typeof(LotOnhand),
                    },
                    new WebModuleMeta()
                    {
                        Label = "库位库存",
                        EntityType = typeof(LocationOnhand),
                    },
                    new WebModuleMeta()
                    {
                        Label = "分配规则",
                        EntityType = typeof(AssignRule),
                    },
                    new WebModuleMeta()
                    {
                        Label = "周转规则",
                        EntityType = typeof(TurnOverRule),
                    },
                    new WebModuleMeta()
                    {
                        Label = "任务分配规则",
                        EntityType = typeof(TaskAllotRule),
                    },
                    new WebModuleMeta()
                    {
                        Label = "单据大类",
                        EntityType = typeof(Function),
                    },
                    new WebModuleMeta()
                    {
                        Label = "单据小类",
                        EntityType = typeof(Transaction),
                    },
                    new WebModuleMeta()
                    {
                        Label = "任务组管理",
                        EntityType = typeof(TaskGroup),
                    },
                    new WebModuleMeta()
                    {
                        Label = "任务管理",
                        EntityType = typeof(TaskManagement),
                    },
                    new WebModuleMeta()
                    {
                        Label = "事务交易",
                        EntityType = typeof(InvTransaction),
                    },
                    new WebModuleMeta()
                    {
                        Label = "垛表",
                        EntityType = typeof(SIE.Inventory.Piles.Pile),
                    });
        }
    }
}
