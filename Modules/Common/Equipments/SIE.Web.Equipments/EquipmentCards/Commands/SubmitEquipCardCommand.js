SIE.defineCommand('SIE.Web.Equipments.EquipmentCards.Commands.SubmitEquipCardCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-Refuse icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) {
            return false;
        }
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ApprovalStatus !== SIE.Equipments.Enums.ApprovalStatus.Draft.value
                && model.data.ApprovalStatus !== SIE.Equipments.Enums.ApprovalStatus.Reject.value) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        SIE.Msg.askQuestion("是否提交？提交后单据不能修改。".t(),
            function () {
                view.execute({
                    withIds: true,
                    selectIds: selectIds,
                    success: function (res) {
                        SIE.Msg.showMessage("提交成功!".t());
                        view.reloadData();
                    }
                });
            });
    }
});