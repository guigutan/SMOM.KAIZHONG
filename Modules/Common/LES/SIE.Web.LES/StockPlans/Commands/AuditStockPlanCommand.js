SIE.defineCommand('SIE.Web.LES.StockPlans.Commands.AuditStockPlanCommand', {
    meta: { text: "审核", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    extends:"SIE.Web.ShipPlan.Commands.AuditDeliveryPlanCommand",
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        if (view.getData().isDirty()) {
            return false;
        }
        var sel = view.getSelection();
        if (sel.any(function (p) { return p.getState() !== SIE.Web.ShipPlan.DeliveryState.Created.value; })) {
            return false;
        } 
        return true;
        //this.callParent(arguments);
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('您确认审核选择的{0}条数据吗?'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("审核成功!".t());
                    view.reloadData();
                }
            });
        });
        //this.callParent(arguments);
    }
});