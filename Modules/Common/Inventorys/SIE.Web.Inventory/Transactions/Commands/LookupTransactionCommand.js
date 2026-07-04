SIE.defineCommand('SIE.Web.Inventory.Transactions.Commands.LookupTransactionCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'TransactionId', targetClassName: 'SIE.Inventory.Transactions.Transaction' },
    },
    meta: { text: "选择".t(), group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections != null && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var transactionId = item.getId();
                if (me._sourceViewSelectItems.indexOf(transactionId) === -1) {
                    var functionTransaction = { FunctionId: me._sourceId, TransactionId: transactionId };
                    operationDatas.push(functionTransaction);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();  //关闭模态窗口
                    me._ownerView.loadChildData(true); //重载视图数据
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
    // end 
});