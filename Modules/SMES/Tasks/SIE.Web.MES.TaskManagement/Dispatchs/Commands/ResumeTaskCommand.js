SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.ResumeTaskCommand', {
    meta: { text: "恢复", group: "edit", hierarchy: "状态", iconCls: "icon-Reload icon-blue" },
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