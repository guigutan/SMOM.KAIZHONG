SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.DeleteEquipAccountAttachmentCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        return (selectModels.length != 0);
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        if (selectIds.length > 1) {
            SIE.Msg.showMessage("只允许删除一条数据!".t());
            return false;
        }
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("删除成功!".t());
                view.reloadData();
            }
        });
    }
});
