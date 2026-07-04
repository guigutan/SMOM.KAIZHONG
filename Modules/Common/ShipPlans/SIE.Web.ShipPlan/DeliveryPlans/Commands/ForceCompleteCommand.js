SIE.defineCommand('SIE.Web.ShipPlan.Commands.ForceCompleteCommand', {
    meta: { text: "强制完成", group: "edit", iconCls: "icon-SettingFinish icon-blue" },

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
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('您确认强制完成操作?'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("操作成功!".t());
                    view.reloadData();
                }
            });
        });
    }
});