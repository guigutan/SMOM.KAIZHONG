SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.MergeTaskCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "合并", group: "edit", iconCls: "icon-Merge icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 1) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.MergedStatus !== 0)
                    return false;
                if (selecteditems[i].data.TaskStatus != 0 && selecteditems[i].data.TaskStatus != 10 ) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    execute: function (view) {
        SIE.Msg.askQuestion(Ext.String.format('是否合并选中任务单？'.t()),
            function () {
                SIE.Msg.wait("正在合并......".t());
                view.execute({
                    withIds: true,
                    selectIds: view.getSelectionIds(),
                    success: function (res) { //回调
                        var errMsg = res.Result;
                        if (errMsg == '合并成功'.t())
                            view.reloadData();
                        SIE.Msg.showMessage(errMsg);
                    }
                });
            });
    }
})