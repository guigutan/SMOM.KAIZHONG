SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageLocationItemLookUpCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ItemId', targetClassName: 'SIE.Items.Item' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections!=null && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var itemId = item.getId();
                if (me._sourceViewSelectItems.indexOf(itemId) === -1) {
                    var storageLocationItemList = { StorageLocationId: me._sourceId, ItemId: itemId };
                    operationDatas.push(storageLocationItemList);
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