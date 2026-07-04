SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentInbounds.Commands.WarehousingCommand', {
    meta: { text: "入库", group: "edit", iconCls: "icon-Import icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.InboundStatus !== 10) {
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
                SIE.Msg.showMessage("入库成功!".t());
                view.reloadData();
            }
        });
    }
});