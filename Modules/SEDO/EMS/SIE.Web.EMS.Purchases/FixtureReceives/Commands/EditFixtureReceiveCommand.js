SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.EditFixtureReceiveCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.ReceiveBillStatus !== 10) return false;
        var detailChildView = view._children.first(function (o) { return o.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
        if (!detailChildView) return false;
        var res = true;
        SIE.each(detailChildView.getData().data.items, function (model) {
            if (model.data.RecivedQty > 0) {
                res = false;
                return false;
            }
        });
        return res;
    }
});