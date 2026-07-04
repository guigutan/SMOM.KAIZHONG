SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.CancelDispatchTaskCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "撤销派工", group: "edit", iconCls: "icon-FileReturn icon-blue" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.TaskStatus != 20) {
                    return false;
                }
            }
            return true;
        }
        return false;
    },
    execute: function (view) {
        SIE.Msg.askQuestion(Ext.String.format('是否撤销派工选中任务单？'.t()),
            function () {
                SIE.Msg.wait("正在撤销派工......".t());
                view.execute({
                    withIds: true,
                    selectIds: view.getSelectionIds(),
                    success: function (res) { //回调
                        var errMsg = res.Result;
                        if (errMsg == '撤销派工成功'.t())
                            view.reloadData();
                        SIE.Msg.showMessage(errMsg);
                    }
                });
            });
    }
})