SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignDisableCommand", {
    meta: { text: "禁用", group: "business", iconCls: "icon-NetworkError icon-red" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        var flag = true;
        sel.forEach(s => {
            if (s.getState() != 1) {
                flag = false;
                return;
            }
        });
        return flag;
    },
    execute: function (view) {
        var selIds = view.getSelectionIds();
        var data = {};
        view.execute({
            data: data,
            withIds: true,
            selectIds: selIds,
            success: function (res) {
                if (res.Success) {
                    SIE.Msg.showInstantMessage("禁用成功".t());
                    view.reloadData();

                }
            }
        })
    },
})