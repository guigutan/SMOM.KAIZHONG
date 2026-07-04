SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonExpRemoveCommand', {
    meta: { text: "移除经验库", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        var model = view.model;
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("移除成功!".t());
                CRT.Event.fire(model + '_refresh');
            }
        });
    }
});