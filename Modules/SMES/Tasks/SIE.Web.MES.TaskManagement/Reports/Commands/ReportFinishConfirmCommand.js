SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.Commands.ReportFinishConfirmCommand', {
    meta: { text: "完工确认", group: "edit", iconCls: "icon-ClipboardPaperCheck icon-blue" },
    canExecute: function (view) {
        var entitys = view.getSelection();
        if (entitys == null) {
            return false;
        }
        if (entitys.length == 0) {
            return false;
        }
        var flag = true;
        entitys.forEach(item => {
            if (item.getTaskStatus() !== 30) {
                flag = false;
            }
        });
        return flag;
    },
    execute: function (view, source) {
        var entityIds = view.getSelectionIds();
        SIE.Msg.wait("正在完工确认......".t());
        view.execute({
            withIds: true,
            selectIds: entityIds,
            success: function (res) { //回调
                if (res.Result) {
                    SIE.Msg.showInstantMessage("完工确认成功".t());
                    view.reloadData();
                }
            }
        })
    }
})