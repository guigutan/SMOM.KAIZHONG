SIE.defineCommand('SIE.Web.Equipments.EquipmentCards.Commands.CancelEquipCardCommand', {
    meta: { text: "撤回", group: "edit", iconCls: "icon-FileReturn icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length === 0) { return false; }
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ApprovalStatus !== SIE.Equipments.Enums.ApprovalStatus.PendingReview.value) {
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
                SIE.Msg.showMessage("撤回成功!".t());
                view.reloadData();
            }
        });
    }
});