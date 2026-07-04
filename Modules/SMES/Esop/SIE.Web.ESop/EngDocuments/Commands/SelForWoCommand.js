SIE.defineCommand('SIE.Web.ESop.EngDocuments.Commands.SelForWoCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.Core.WorkOrders.WorkOrder' }
    },
    meta: { text: "适用工单", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },

    canExecute: function (view) {
        var selection = view.getSelection();
        if (selection.length <= 0) {
            return false;
        }
        return true;
    },
    save: function (win) {
        var me = this;
        var woIds = "";
        var woNos = "";
        var selectWo = me._targetSelectItems;
        var data = me.view.getSelection();
        for (var i = 0; i < selectWo.items.length; i++) {
            woIds += selectWo.items[i].getId();
            woNos += selectWo.items[i].getNo();
            if (i != (selectWo.items.length - 1)) {
                woIds += ';';
                woNos += ';';
            }
        }
        data.forEach(p => {
            p.setWoIds(woIds);
            p.setWoNos(woNos);
        });
        win.close();
    },
    _popupWin: function (ui, source) {
        /// <summary>
        /// 弹窗口
        /// </summary>
        /// <param name="ui" type="type"></param>
        /// <param name="source" type="type"></param>
        var me = this;
        me._targetView = ui._view;
        if (me.win && me.win.animateTarget == source) {
            return;
        }
        //弹窗
        me.win = SIE.Window.show({
            title: ('选择' + me._targetView.label).t(),
            animateTarget: source, items: ui.getControl(),
            width: 800, height: 420,
            listeners: {
                close: function () {
                    me.lastClickTime = 0;
                }
            },
            // buttons: ['清空','确定', '关闭'],
            callback: function (btn) {
                if (btn === '确定'.t()) {
                    var elapsed = Ext.now() - me.lastClickTime;
                    var interval = me.getExecuteInterval();
                    if (elapsed >= interval) {
                        me.lastClickTime = Ext.now();
                        if (me._targetSelectItems.keys.length > 0) {
                            me.save(me.win);
                            return false; //阻止窗口关闭，在save中根据返回结果处理
                        } else {
                            SIE.Msg.showWarning('没有可提交的数据'.t());   //没有选择数据点击确定时，窗口直接关闭了
                            return false;
                        }
                    }
                    return false;
                }
            }
        });
        SIE.Window.winAutoSize(me.win);
        me.setGridListeners();
        me._targetSelectItems = { items: [], keys: [] };
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        var me = this;
        if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)
            || (me._targetSelectItems && me._targetSelectItems.items.length > 0)) {
            // 弹框
            var selModel = me._targetView.getSelectionModel();
            var detailSelections = me.view.getSelection()
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._sourceViewSelectItems.indexOf(record.getId()) > -1) {
                        selModel.select(record, true, true); //勾选上.
                    }
                    if (me._targetSelectItems.keys.indexOf(record.getId()) > -1) {
                        selModel.select(record, true, true);
                    }
                }
            }
        }
    },
});
