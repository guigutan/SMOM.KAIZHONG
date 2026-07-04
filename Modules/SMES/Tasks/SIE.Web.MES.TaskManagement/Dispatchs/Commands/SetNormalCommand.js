SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.SetNormalCommand', {
    meta: { text: "普通", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.Priority !== 1) {
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
                if (errMsg == '设置普通成功'.t())
                    view.reloadData();
                SIE.Msg.showMessage(errMsg);
            }
        });
    }
});