SIE.defineCommand('SIE.Web.Barcodes.WipBatchs.Commands.DeleteWipBatchCommand', {
    meta: { text: "删除批次", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getCurrent() == null)
            return false;
        if (view.getSelection().length < 1)
            return false;
        return true;
    },
    execute: function (view, source) {
        var curIds = view.getSelectionIds();
        var datas = [];
        for (var i = 0; i < curIds.length;i++) {
            datas.push(curIds[i]);
        }

        var msg = Ext.String.format('你确定删除选择的{0}条数据吗？确定后将直接删除'.L10N(), curIds.length);
        SIE.Msg.askQuestion(msg, function () {

            SIE.invokeDataQuery({
                method: 'DeleteWipBatch',
                params: [datas],
                action: 'queryer',
                type: 'SIE.Web.MES.BatchWIP.Products.BatchWipDataQueryer',
                token: view.token,
                success: function (res) {
                    view.reloadData();
                    view._parent.reloadData();
                    SIE.Msg.showInstantMessage('删除批次成功'.t());
                }
            });
        });
    }
});