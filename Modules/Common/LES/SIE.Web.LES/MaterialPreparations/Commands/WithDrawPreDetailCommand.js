SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.WithDrawPreDetailCommand", {
    meta: { text: "取消", group: "edit", iconCls: "icon-FileReturn icon-red" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length <= 0) {
            return false;
        }
        if (sel.find(p => p.getPreDetailStatus() != 1)) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var selIds = view.getSelectionIds();
        view.execute({
            withIds: true,
            selectIds: selIds,
            success: function (res) {
                if (res.Success) {
                    SIE.Msg.showMessage("取消成功".t());
                    view.getParent().reloadData();
                }
            }
        })
    }
})