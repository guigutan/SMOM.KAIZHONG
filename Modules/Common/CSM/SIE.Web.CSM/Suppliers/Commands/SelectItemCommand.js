
SIE.defineCommand('SIE.Web.CSM.Suppliers.Commands.SelectItemCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ItemId', targetClassName: 'SIE.Items.Item' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var operationDatas = [];
            SIE.each(selections.items, function (item) {
                var itemId = item.getId();
                if (me._sourceViewSelectItems.indexOf(itemId) === -1) {
                    var supplierItem = { SupplierId: me._sourceId, ItemId: itemId };
                    operationDatas.push(supplierItem);
                }
            });
            if (operationDatas.length > 0) {
                indata = operationDatas;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        win.close();  //关闭模态窗口
                        me._ownerView.loadChildData(true); //重载视图数据
                    }
                }, me._ownerView);
                return true;
            }
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});