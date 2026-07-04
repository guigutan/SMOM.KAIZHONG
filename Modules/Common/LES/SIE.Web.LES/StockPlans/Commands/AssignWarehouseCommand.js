SIE.defineCommand('SIE.Web.LES.StockPlans.Commands.AssignWarehouseCommand', {
    extend: 'SIE.Web.ShipPlan.Commands.AssignWarehouseCommand',
    meta: { text: "分配仓库", group: "edit", iconCls: "icon-LoopCheck icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        if (view.getData().isDirty()) {
            return false;
        }
        var sel = view.getSelection();
        if (sel.any(function (p) {
            return p.getState() !== SIE.Web.ShipPlan.DeliveryState.Created.value &&
                p.getState() !== SIE.Web.ShipPlan.DeliveryState.Aduited.value &&
                p.getState() !== SIE.Web.ShipPlan.DeliveryState.Executing.value;
        })) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('您确认分配仓库操作?'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("分配成功!".t());
                    view.reloadData();
                }
            });
        });
        //this.callParent(arguments);
    }
});