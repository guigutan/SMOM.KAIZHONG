SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.Commands.ReportTaskResumeCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "恢复", group: "edit", iconCls: "icon-Reload icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.TaskStatus !== 40) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    execute: function (view, source) {
        SIE.Msg.wait("正在恢复......".t());
        view.execute({
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) { //回调
                var errMsg = res.Result;
                if (errMsg == '恢复成功'.t())
                    view.reloadData();
                SIE.Msg.showMessage(errMsg);
            }
        });
    }
});