/*
 **条码关联选择弹出命令
 */
SIE.defineCommand('SIE.Web.MES.WorkOrders.UnionBarcodeSelCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'OriginalBarcode', targetClassName: 'SIE.MES.WorkOrders.Reworks.UnionBarcodeCore', targetCriteriaClassName: 'SIE.MES.WorkOrders.Reworks.UnionBarcodeViewCriteria' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (listView) {
        return true;
    },
    save: function (win) {
        var me = this;
        var selections = this._targetSelectItems.items;
        var barcodes = [];
        for (var i = 0; i < selections.length; i++) {
            barcodes.push(selections[i].data.Barcode);
        }
        me.updateUnionBarcode(barcodes, this._ownerView, win);
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = [];
        Ext.each(me._ownerView.getData().data.items, function (item) {
            me._sourceViewSelectItems.push(item.data.OriginalBarcode);
        })
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        var me = this;
        Ext.each(records, function (item) {
            Ext.each(me._ownerView.getData().data.items, function (selitem) {
                if (selitem.data.OriginalBarcode == item.data.Barcode)
                    me._sourceViewSelectItems.push(item.data.Id);
            })
        })
        if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)
            || (me._targetSelectItems && me._targetSelectItems.items.length > 0)) {
            var selModel = me._targetView.getSelectionModel();
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._sourceViewSelectItems.indexOf(record.getBarcode()) > -1) {
                        selModel.select(record, true, true); //勾选上.
                    }
                    if (me._targetSelectItems.keys.indexOf(record.getBarcode()) > -1) {
                        selModel.select(record, true, true);
                    }
                }
            }
        }
    },
    _popupWin: function (ui, source) {
        var me = this;
        me._targetView = ui._view;
        if (me.win && me.win.animateTarget == source) {
            return;
        }
        //弹窗
        me.win = SIE.Window.show({
            title: ('条码 选择').t(),
            animateTarget: source,
            items: ui.getControl(),
            width: 1180,
            height: 600,
            //buttons: ['确定', '关闭'], //自定义按钮名称
            callback: function (btn) {
                if (btn === '确定'.t()) {
                    if (me._targetSelectItems.keys.length > 0) {
                        me.save(me.win);
                        return false; //阻止窗口关闭，在save中根据返回结果处理
                    } else {
                        SIE.Msg.showWarning('没有可提交的数据'.t()); //没有选择数据点击确定时，窗口直接关闭了
                        return false;
                    }
                }
            }
        });

        me.setGridListeners();
        me._targetSelectItems = { items: [], keys: [] };
    },
    updateUnionBarcode: function (barcodes, listView, win) {
        var woBarcode = listView._parent;
        var entityData = listView._parent.getData().data;
        var unionBarcode = [];
        Ext.each(listView.getData().data.items, function (item) {
            item.data.CreateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            item.data.UpdateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            if (barcodes.indexOf(item.data.OriginalBarcode) > -1) {
                barcodes.remove(item.data.OriginalBarcode);
            }
            unionBarcode.push(item.data);
        });

        var keyItems = [];
        Ext.each(woBarcode._children[1].getData().data.items, function (item) {
            item.data.CreateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            item.data.UpdateDate = Ext.Date.format(new Date(), "Y-m-d H:i:s");
            keyItems.push(item.data);
        });
        if (barcodes.length == 0) { win.close(); return; }
        if (entityData.PlanQty < listView.getData().data.items.length + barcodes.length) {
            SIE.Msg.showError(Ext.String.format('已超过工单计划数量：{0}'.L10N(), entityData.PlanQty));
            return false;
        }


        SIE.invokeDataQuery({
            method: 'GetUnionBarcodes',
            params: [woBarcode.getData().data, unionBarcode, keyItems, barcodes],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.Reworks.ReworkDataQueryer',
            token: listView.token,
            success: function (res) {
                var re = res.Result;
                if (re.Error != null && re.Error != "") {
                    SIE.Msg.showError((re.Error));
                }
                var barList = listView.getControl();
                var store = barList.getStore();
                store.setData(re.BarcodeList);
                barList.setStore(store);

                var keyList = listView._parent._children[1].getControl();
                var keyStore = keyList.getStore();
                keyStore.setData(re.KeyItemList)
                keyList.setStore(keyStore);

                woBarcode.getData().setScanQty(re.ScanQty);
                win.close();
            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
                return false;
            }
        });

    },
});