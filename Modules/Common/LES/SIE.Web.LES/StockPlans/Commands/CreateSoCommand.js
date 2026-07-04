SIE.defineCommand('SIE.Web.LES.StockPlans.Commands.CreateSoCommand', {
    meta: { text: "创建发运订单", group: "edit", iconCls: "icon-PaperPlane icon-blue" },
    extends:"SIE.Web.ShipPlan.Commands.CreateSoCommand",
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        if (view.getData().isDirty()) {
            return false;
        }
        var sel = view.getSelection();
        if (sel.any(function (p) {
            return p.getState() !== SIE.Web.ShipPlan.DeliveryState.Aduited.value &&
                p.getState() !== SIE.Web.ShipPlan.DeliveryState.Executing.value;
        }))
            return false;
        return true;
        //this.callParent(arguments);
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('您确认进行创建发运订单操作?'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("创建发运订单成功!".t());
                    view.reloadData();
                },
                error: function (res) {
                    view.reloadData();
                }
            });
        });
        //this.call
        //this.callParent(arguments);
    }
});