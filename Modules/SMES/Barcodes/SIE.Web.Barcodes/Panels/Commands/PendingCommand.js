SIE.defineCommand('SIE.Web.Barcodes.Panels.Commands.PendingCommand', {
    meta: { text: "挂起", group: "edit", iconCls: "icon-Refuse icon-red" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.IsPending === true) {
                res = false;
            }
        });
        return res;
    },
    execute: function (listView, source) {

        SIE.Msg.askQuestion(Ext.String.format('是否确认挂起?'.t()), function () {
            listView.execute({
                data: { BarCodeIds: listView.getSelectionIds() },
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("挂起成功!".t());
                    listView._parent.reloadData();
                }
            });
        });
        
    }
});