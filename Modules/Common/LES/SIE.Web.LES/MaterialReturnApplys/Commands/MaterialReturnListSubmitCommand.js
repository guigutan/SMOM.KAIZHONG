SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.MaterialReturnListSubmitCommand", {
    meta: { text: "提交", group: "edit", iconCls: "icon-Submit icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        if (sel.find(p => p.getReStatus() != 0)) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var selIds = view.getSelectionIds();
        SIE.Msg.wait("正在提交中,请稍等......".t());
        view.execute({
            withIds: true,
            selectIds: selIds,
            success: function (res) { //回调
                SIE.Msg.showMessage("提交成功".t());
                view.reloadData();
            }
        });
    }
})