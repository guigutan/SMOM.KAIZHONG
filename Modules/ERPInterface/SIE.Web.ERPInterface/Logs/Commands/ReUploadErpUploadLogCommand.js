SIE.defineCommand('SIE.Web.ERPInterface.Logs.Commands.ReUploadErpUploadLogCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "重新上传", group: "edit", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        var transList = view.getSelection();
        if (transList == null || transList.length == 0) {
            return false;
        }

        var trans = view.getCurrent();
        if (trans == null) {
            return false;
        }
        if (transList.any(function (f) { return f.data.IsSuccess || f.data.UploadTransactionState != 2 }))
            return false;

        return true;
    },
    execute: function (view, source) {
        var transList = view.getSelection();
        var uptranIds = transList.select(function (f) { return f.data.UploadTransactionId });
        SIE.Msg.askQuestion(Ext.String.format('确定[重新上传]选中的事务?'.t()), function () {
            view.execute({
                data: view.getCurrent().data,
                withIds: true,
                selectIds: uptranIds,
                success: function (res) { //回调
                    view.reloadData();
                    SIE.Msg.showInstantMessage("操作成功，记录已更新为重试，待调度上传。".t());
                }
            });
        });
    }
});