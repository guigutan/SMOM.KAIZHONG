SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignEnableCommand",{
    meta: { text: "启用", group: "business", iconCls: "icon-NetworkNormal icon-green" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        var flag = true;
        sel.forEach(s => {
            if (s.getExamineStatus() != 1 || s.getState() != 0) {
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
                    SIE.Msg.showInstantMessage("启用成功".t());
                    view.reloadData();

                }
            }
        })
    },
})