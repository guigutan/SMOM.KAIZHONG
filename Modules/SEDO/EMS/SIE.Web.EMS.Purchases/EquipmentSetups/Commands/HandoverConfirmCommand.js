SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.HandoverConfirmCommand', {
    meta: { text: "交机确认", group: "edit", iconCls: "icon-EnableUsers icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.SetupStatus !== 30) {
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
                SIE.Msg.showMessage("交机确认成功!".t());
                view.reloadData();
            }
        });
    }
});