Ext.define('SIE.Web.EMS.Control.IdLinkComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.IdLinkComboPopup',

    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        var view = me.up("form").SIEView;
        var entity = view.getData();
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    me.setQueryCriteria(dialogView, entity.data);
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },

    /// <summary>
    /// 库区、库位编码存在重复，所以用另外一个ID字段来设置勾选
    /// </summary>
    onLoad: function (store, records, successful, operation, eOpts) {
        var me = this;
        if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)) {
            var selModel = me._targetView.getSelectionModel();
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._sourceViewSelectItems.indexOf(record.getId()) > -1) {
                        selModel.select(record, true, true); //勾选上.
                        if (Ext.Array.indexOf(me._targetSelectItems.keys, record.getId(), 0) === -1) {
                            me._targetSelectItems.keys.push(record.getId());
                            me._targetSelectItems.items.push(record);
                        }
                    }
                }
            }
        }
    },
    setQueryCriteria: function (dialogView, data) {

    },

    _setWinListeners: function () {
        var me = this;
        me._win.on('focusleave',
            function (vthis, event, eOpts) {
                if (!event.toElement || me._win.owns(event.toElement) === false) {
                    if (me._win.hasFocus === false) {
                        me._win.hide();
                        me._winNum = 0;
                    }
                }
            },
            me._win, {
            delay: 50
        }); //可能移光标到 主控件
        me.on('focusleave',
            function (vthis, event, eOpts) {
                if (!event) return;

                if (me._win && event.toElement && me._win.owns(event.toElement) === false) {
                    me._win.hide();
                    me._winNum = 0;
                }
            },
            me, {
            delay: 50
        });

        me._win.on('hide',
            function () {
                me._doAutoSelectOnClose();
                delete me.isCanceling;
            });

        me._win.on('show',
            function () {
                if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)) {
                    var selModel = me._targetView.getSelectionModel();
                    if (selModel.store.data && selModel.store.data.length > 0) {
                        for (var i = 0, len = selModel.store.data.length; i < len; i++) {
                            var record = selModel.store.data.items[i];
                            if (me._sourceViewSelectItems.indexOf(record.getId()) > -1) {
                                selModel.select(record, true, true); //勾选上.
                                if (Ext.Array.indexOf(me._targetSelectItems.keys, record.getId(), 0) === -1) {
                                    me._targetSelectItems.keys.push(record.getId());
                                    me._targetSelectItems.items.push(record);
                                }
                            }
                        }
                    }
                }
            },
            me, {
            delay: 50
        });
    },
});