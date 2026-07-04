SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignAgainstExamineCommand", {
    meta: { text: "反审", group: "edit", iconCls: "iconfont icon-FileReturn icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        var flag = true;
        sel.forEach(s => {
            if (s.getExamineStatus() != 1) {
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
                    SIE.Msg.showInstantMessage("反审成功".t());
                    view.reloadData();

                }
            }
        })
    },
})