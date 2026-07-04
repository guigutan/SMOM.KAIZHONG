SIE.defineCommand('SIE.Web.Barcodes.BarcodeResumeCommand', {
    meta: { text: "恢复", group: "edit", iconCls: "icon-Reload icon-blue" },

    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.IsScraped === true || model.data.IsPending === false) {
                res = false;
            }
        });
        return res;
    },
    execute: function (listView) {
        var selectModels = listView.getSelectionIds();
        var msg = Ext.String.format('你确定恢复选择的{0}条数据吗？'.L10N(), selectModels.length);

        SIE.Msg.askQuestion(msg, function () {
            listView.execute({
                withIds: true,
                selectIds: selectModels,
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage('恢复成功'.t());
                    var selectedEntities = listView.getSelectedEntities();
                    listView.unSelectEntities(selectedEntities);
                    listView.reloadData();
                }
            });
        });
    }
});