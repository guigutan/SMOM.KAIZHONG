SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignExamineCommand", {
    meta: { text: "审核", group: "edit", iconCls: "iconfont icon-NetworkNormal icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        var flag = true;
        sel.forEach(s => {
            if (s.getRoutingInfo() != 1 || s.getBomInfo() != 1 || s.getExamineStatus() != 0) {
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
                    SIE.Msg.showInstantMessage("审核成功".t());
                    view.reloadData();

                }
            }
        })
    },
})