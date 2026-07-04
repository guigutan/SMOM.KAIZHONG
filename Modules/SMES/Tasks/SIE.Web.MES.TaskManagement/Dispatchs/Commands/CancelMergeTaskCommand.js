SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.CancelMergeTaskCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "取消合并", group: "edit", iconCls: "icon-Undo icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (!(selecteditems[i].data.TaskStatus == 0 || selecteditems[i].data.TaskStatus == 10) || selecteditems[i].data.MergedStatus !== 2) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    execute: function (view) {
        SIE.Msg.askQuestion(Ext.String.format('是否取消合并选中任务单？'.t()),
            function () {
                SIE.Msg.wait("正在取消合并......".t());
                view.execute({
                    withIds: true,
                    selectIds: view.getSelectionIds(),
                    success: function (res) { //回调
                        var errMsg = res.Result;
                        if (errMsg == '取消合并成功'.t())
                            view.reloadData();
                        SIE.Msg.showMessage(errMsg);
                    }
                });
            });
    }
})