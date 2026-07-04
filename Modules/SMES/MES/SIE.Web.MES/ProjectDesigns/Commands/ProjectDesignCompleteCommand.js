SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignCompleteCommand", {
    meta: { text: "设计完成", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        var flag = true;
        sel.forEach(s => {
            if (s.getRoutingInfo() != 1 || s.getBomInfo() != 1) {
                flag = false;
                return;
            }
        });
        return flag;
    },
    execute: function (view) {
        var selIds = view.getSelectionIds();
        var data = {};
        SIE.Msg.wait("正在确认......".t());
        view.execute({
            data: data,
            withIds: true,
            selectIds: selIds,
            success: function (res) {
                if (res.Success) {
                    SIE.Msg.showInstantMessage("设计完成".t());
                    view.reloadData();
                    
                }
            }
        })
    }
})