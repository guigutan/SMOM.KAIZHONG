SIE.defineCommand('SIE.Web.MES.WorkOrders.Reworks.RemoveBarcodeCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "移除条码", group: "edit", iconCls: "icon-Delete icon-blue" },
    canExecute: function (listView) {
        if (listView == null || listView.getCurrent() == null || listView.getSelection().length < 1) return false;
        if (listView.getSelection().where(function (p) { return p.data.CodeState == 0; }).length > 0) return false
        else
            return true;
    },
    execute: function (listView) {
        var me = this;
        var removes = listView.getSelection();
        var woBarcode = listView._parent;

        var removeBarcodes = [];
        Ext.each(listView.getSelection(), function (item) {
            removeBarcodes.push(item.data.OriginalBarcode);
        });

        var unionBarcode = [];
        Ext.each(listView.getData().data.items, function (item) {
            item.data.CreateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            item.data.UpdateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            unionBarcode.push(item.data);
        });

        var keyItems = [];
        Ext.each(woBarcode._children[1].getData().data.items, function (item) {
            item.data.CreateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            item.data.UpdateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            keyItems.push(item.data);
        });

        SIE.invokeDataQuery({
            method: 'RemoveUnionBarcodes',
            params: [woBarcode.getData().data, unionBarcode, keyItems, removeBarcodes],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.Reworks.ReworkDataQueryer',
            token: listView.token,
            success: function (res) {
                var re = res.Result;
                var barList = listView.getControl();
                var store = barList.getStore();
                store.setData(re.BarcodeList);
                barList.setStore(store);

                var keyList = listView._parent._children[1].getControl();
                var keyStore = keyList.getStore();
                keyStore.setData(re.KeyItemList)
                keyList.setStore(keyStore);

                listView._parent.getData().setScanQty(re.ScanQty);
                return true;
            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
                return false;
            }
        });
    }
});