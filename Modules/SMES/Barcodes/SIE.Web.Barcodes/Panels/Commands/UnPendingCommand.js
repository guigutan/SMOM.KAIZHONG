SIE.defineCommand('SIE.Web.Barcodes.Panels.Commands.UnPendingCommand', {
    meta: { text: "恢复", group: "edit", iconCls: "icon-Receive icon-red" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.IsPending === false) {
                res = false;
            }
        });
        return res;
    },
    execute: function (listView, source) {

        SIE.Msg.askQuestion(Ext.String.format('是否恢复挂起?'.t()), function () {
            listView.execute({
                data: { BarCodeIds: listView.getSelectionIds() },
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("恢复成功!".t());
                    listView._parent.reloadData();
                }
            });
        });
        
    }
});