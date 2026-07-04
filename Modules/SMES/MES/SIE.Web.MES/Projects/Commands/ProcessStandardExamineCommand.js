SIE.defineCommand("SIE.Web.MES.Projects.Commands.ProcessStandardExamineCommand", {
    meta: { text: "审核", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        var flag = true;
        if (sel == null || sel.length <= 0)
        {
            return false;
        }
        Ext.each(sel, function (item) {
            if (item.getProcessStStatus() != 1) {
                flag = false;
                return;
            }
        });
        return flag;
    },
    execute: function (view) {
        view.execute({
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) { //回调
                if (res.Success) {
                    SIE.Msg.showInstantMessage('审核成功'.t());
                    view.reloadData();
                }
            }
        })
    }
})