SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.SetUrgentCommand', {
    meta: { text: "紧急", group: "edit", iconCls: "icon-AlertCircle icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.Priority !== 0) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    execute: function (view, source) {
        SIE.Msg.wait("正在设置优先级......".t());
        view.execute({
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) { //回调
                var errMsg = res.Result;
                if (errMsg == '设置紧急成功'.t())
                    view.reloadData();
                SIE.Msg.showMessage(errMsg);
            }
        });
    }
});