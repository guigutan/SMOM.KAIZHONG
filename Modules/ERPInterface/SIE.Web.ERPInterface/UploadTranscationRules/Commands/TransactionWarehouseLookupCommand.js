SIE.defineCommand('SIE.Web.ERPInterface.UploadTransactionRules.Commands.TransactionWarehouseLookupCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'WarehouseId', targetClassName: 'SIE.Warehouses.Warehouse' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections!=null && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var warehouseId = item.getId();
                if (me._sourceViewSelectItems.indexOf(warehouseId) === -1) {
                    var transcWarehouse = { UploadTransactionRuleId: me._sourceId, WarehouseId: warehouseId };
                    operationDatas.push(transcWarehouse);
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