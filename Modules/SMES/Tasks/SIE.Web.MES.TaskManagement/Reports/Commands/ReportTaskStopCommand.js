SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.Commands.ReportTaskStopCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "暂停", group: "edit", iconCls: "icon-Pause icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.TaskStatus !== 30) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    execute: function (view, source) {
        SIE.Msg.wait("正在暂停......".t());
        view.execute({
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) { //回调
                var errMsg = res.Result;
                if (errMsg == '暂停成功')
                    view.reloadData();
                SIE.Msg.showMessage(errMsg.t());
            }
        });
    },
});
