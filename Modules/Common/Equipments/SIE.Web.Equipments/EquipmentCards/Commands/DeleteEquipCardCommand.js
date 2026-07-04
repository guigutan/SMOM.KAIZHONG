SIE.defineCommand('SIE.Web.Equipments.EquipmentCards.Commands.DeleteEquipCardCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        var res = false;
        SIE.each(selectModels, function (model) {
            if (model.data.EquipmentCardSource === 70
                && model.data.CreateCardDateTime === null
                && (model.data.ApprovalStatus === SIE.Equipments.Enums.ApprovalStatus.Draft.value
                    || model.data.ApprovalStatus === SIE.Equipments.Enums.ApprovalStatus.Reject.value)) {
                res = true;
                return true;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？确认后直接删除！'.t(), sel.length), function () {
            view.execute({
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) {
                    view.removeSelection();
                    view.setCurrent(null, true);
                    view.reloadData();
                },
            });
        });
    }
});