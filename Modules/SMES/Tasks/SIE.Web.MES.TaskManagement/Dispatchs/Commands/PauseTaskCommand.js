SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.PauseTaskCommand', {
    meta: { text: "暂停", group: "edit", hierarchy: "状态", iconCls: "icon-Pause icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.TaskStatus === 40 || selecteditems[i].data.TaskStatus === 50 || selecteditems[i].data.TaskStatus === 60) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    execute: function (view, source) {
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