SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.CompletePurDetailCommand', {
    meta: { text: "完成", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        var parent = view._parent.getCurrent();
        if (parent.data.PurchaseOrderStatus !== 10 && parent.data.PurchaseOrderStatus !== 20)
            return false;
        if (parent.data.ApprovalStatus !== 40)
            return false;
        SIE.each(selectModels, function (model) {
            if (model.data.Status !== 10 && model.data.Status !== 20) {
                res = false;
                return false;
            }
            if (model.data.ReciveQty > 0 || model.data.AcceptanceQty > 0) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("完成成功!".t());
                view.reloadData();
            }
        });
    }
});