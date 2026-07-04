SIE.defineCommand('SIE.Web.ERPInterface.Logs.Commands.CloseUploadErpUploadLogCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "关闭上传", group: "edit", iconCls: "icon-CloseView icon-red" },

    canExecute: function (view) {
        var transList = view.getSelection();
        if (transList == null || transList.length == 0) {
            return false;
        }

        var trans = view.getCurrent();
        if (trans == null) {
            return false;
        }
        //重试或失败的，可以关闭上传
        if (transList.any(function (f) { return f.data.IsSuccess || f.data.UploadTransactionState != 3 && f.data.UploadTransactionState != 2; }))
            return false;

        return true;
    },
    execute: function (view, source) {
        var transList = view.getSelection();
        var uptranIds = transList.select(function (f) { return f.data.UploadTransactionId });
        SIE.Msg.askQuestion(Ext.String.format('确定[关闭上传]选中的事务?'.t()), function () {
            view.execute({
                withChildren: true,
                withIds: true,
                selectIds: uptranIds,
                success: function (res) {
                    view.removeSelection();
                    view.setCurrent(null);
                    view.reloadData();
                    SIE.Msg.showInstantMessage("操作成功".t());
                },
            });
        });
    }
});