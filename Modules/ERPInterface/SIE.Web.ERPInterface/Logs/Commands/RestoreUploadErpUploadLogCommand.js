SIE.defineCommand('SIE.Web.ERPInterface.Logs.Commands.RestoreUploadErpUploadLogCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "恢复上传", group: "edit", iconCls: "icon-ArrowLongUp icon-red" },

    canExecute: function (view) {
        var transList = view.getSelection();
        if (transList == null || transList.length == 0) {
            return false;
        }

        var trans = view.getCurrent();
        if (trans == null) {
            return false;
        }
        if (transList.any(function (f) { return f.data.IsSuccess || f.data.State != 5; }))
            return false;

        return true;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定恢复选择的{0}条数据吗？确认后直接恢复'.t(), sel.length), function () {
            view.execute({
                withChildren: true,
                withIds: true,
                selectIds: view.getSelectionIds(),
                data: view.getSelectionIds(),
                success: function (res) {
                    view.removeSelection();
                    view.setCurrent(null);
                    view.reloadData();
                },
            });
        });
    }
});