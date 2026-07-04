SIE.defineCommand('SIE.Web.MES.WorkOrders.Reworks.UnionBarcodeSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    execute: function (view) {
        var me = this;

        var woId = view.getData().data.WorkOrderId;
        var unionBarcode = [];
        Ext.each(view._children[0].getData().data.items, function (item) {
            item.data.CreateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            item.data.UpdateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            unionBarcode.push(item.data);
        });

        var keyItems = [];
        Ext.each(view._children[1].getData().data.items, function (item) {
            item.data.CreateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            item.data.UpdateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            keyItems.push(item.data);
        });

        var indata = {};
        indata.WorkOrderId = woId;
        indata.BarcodeList = unionBarcode;
        indata.KeyItemList = keyItems;
        view.execute({
            data: indata,
            success: function (res) {
                var current = view.getCurrent();
                current.markSaved();
                SIE.Msg.showMessage('保存成功！');
                me.initChildrenData(view);               
            }
        });
    },
    initChildrenData: function (curView) {
        var me = this;
        if (curView._children.length === 0)
            return;
        SIE.invokeDataQuery({
            method: 'GetUnionBarcodeData',
            params: [curView.getData().data.WorkOrderId, curView.getData().data.WorkOrderNo],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.Reworks.ReworkDataQueryer',
            token: curView.token,
            success: function (res) {
                var re = res.Result;
                var unionBarcodeView = curView._children.first(function (child) { return child.model === 'SIE.MES.WorkOrders.Reworks.UnionBarcode' });
                if (unionBarcodeView) {
                    var barList = unionBarcodeView.getControl();
                    var store = barList.getStore();
                    store.setData(re.BarcodeList);
                    barList.setStore(store);
                    barList.store.data.each(function (p) { p.commit(); });
                }
                var keyItemView = curView._children.first(function (child) { return child.model === 'SIE.MES.WorkOrders.Reworks.KeyItemUnboundConfig' });
                if (keyItemView) {
                    var keyList = keyItemView.getControl();
                    var keyStore = keyList.getStore();
                    keyStore.setData(re.KeyItemList);
                    keyList.setStore(keyStore);
                    keyList.store.data.each(function (p) { p.commit(); });
                }
                curView.getData().setRelevancyQty(re.BarcodeList.length);
                curView.getData().setScanQty(null);
                curView.getCurrent().markSaved();
                curView.syncCmdState();    //刷新按钮状态

            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
                return false;
            }
        });
    }
});