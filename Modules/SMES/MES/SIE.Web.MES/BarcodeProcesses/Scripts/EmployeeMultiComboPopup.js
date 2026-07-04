Ext.define('SIE.Web.MES.BarcodeProcesses.Scripts.EmployeeMultiComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.EmployeeMultiComboPopup',

    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
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
        var me = this;
        if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)) {
            var selModel = me._targetView.getSelectionModel();
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._sourceViewSelectItems.indexOf(record.getId()) > -1) {
                        selModel.select(record, true, true);
                        if (Ext.Array.indexOf(me._targetSelectItems.keys, record.getId(), 0) === -1) {
                            me._targetSelectItems.keys.push(record.getId());
                            me._targetSelectItems.items.push(record);
                        }
                    }
                }
            }
        }
    },
    //触发器事件
    onTriggerClick: function (field, trigger, e) {
        var me = this;
        if (field.readOnly)
            return;
        if (me._winNum == 0) {
            me._winNum = 1;
            me._sourceViewSelectItems = [];
            var entity = null;

            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            }
            else {
                entity = me.up("container").context.record;
            }

            if (entity.data.EmployeeIds != "" && entity.data.EmployeeIds != null) {
                entity.data.EmployeeIds.split(me.separator).forEach(function (item) {
                    if (item) {
                        me._sourceViewSelectItems.push(parseFloat(item));
                    }
                });
            }

            me._createLayout(field);
        }
    },

    onpopupWinbtn: function (btn) {
        var me = this;
        if (btn === '确定'.t()) {
            var entity;
            me.setMULTIValue();
            if (!me.up("form"))
                entity = me.up("container").context.record;
            else
                entity = me.up("form").SIEView.getData();

            var displayVal2 = "";
            me._targetSelectItems.items.forEach(function (model) {
                displayVal2 += me.separator + model.data[me.displayField];
            });
            displayVal2 = displayVal2.substring(me.separator.length);
            entity.setEmployeeJoinNames(displayVal2);
            me._win.hide();
            return true; //阻止窗口关闭，在save中根据返回结果处理
        } else if (btn === '取消'.t()) {
            me.isCanceling = true;
            return true;
        }
    }
});
