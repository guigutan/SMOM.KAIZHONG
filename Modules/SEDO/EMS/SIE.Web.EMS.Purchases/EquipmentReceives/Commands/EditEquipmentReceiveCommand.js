SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentReceives.Commands.EditEquipmentReceiveCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;

        //改成待接收时，才可以修改
        if (p.data.ReceiveBillStatus !== 0) return false;

        var detailChildView = view._children.first(function (o) { return o.model === "SIE.EMS.Purchases.EquipmentReceives.EquipmentReceiveDetail"; });
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