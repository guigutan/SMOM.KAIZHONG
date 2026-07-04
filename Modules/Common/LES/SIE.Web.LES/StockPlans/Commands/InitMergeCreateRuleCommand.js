SIE.defineCommand('SIE.Web.LES.StockPlans.Commands.InitMergeCreateRuleCommand', {
    meta: { text: "初始化", group: "edit", iconCls: "iconfont icon-Reload icon-blue" },
    extends:"SIE.Web.ShipPlan.Commands.InitMergeCreateRuleCommand",
    canExecute: function (view) {
        var curdata = view.getData();
        if (curdata != null) {
            for (i = 0, len = curdata.data.items.length; i < len; i++) {
                var item = curdata.data.items[i];
                if (item.data.OrderType === SIE.Inventory.Commom.OrderType.SaleOut.value || item.data.OrderType === SIE.Inventory.Commom.OrderType.WorkFeed.value ||
                    item.data.OrderType === SIE.Inventory.Commom.OrderType.OutWorkFeedUse.value || item.data.OrderType === SIE.Inventory.Commom.OrderType.OutAllotReturn.value ||
                    item.data.OrderType === SIE.Inventory.Commom.OrderType.OutWorkFeed.value || item.data.OrderType === SIE.Inventory.Commom.OrderType.OtherOut.value ||
                    item.data.OrderType === SIE.Inventory.Commom.OrderType.WhTransferOut.value ||
                    item.data.OrderType === SIE.Inventory.Commom.OrderType.SupplierReturn.value) {
                    return false;
                }
            }
        }
        return true;
        //this.callParent(arguments);
    },
    execute: function (view, source) {
        view.execute({
            data: view.model,
            success: function (res) { //回调
                view.reloadData();
            }
        });
        //this.callParent(arguments);
    }
});