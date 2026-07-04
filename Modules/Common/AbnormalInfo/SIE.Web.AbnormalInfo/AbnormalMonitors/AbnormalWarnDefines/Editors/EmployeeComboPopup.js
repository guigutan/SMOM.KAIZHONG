Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.Editors.EmployeeComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.employeecombopopup',
    /**
  * 确定事件
  * @param btn--
  * @returns
  */
    onpopupWinbtn: function (btn) {
        var me = this;
        if (btn === '确定'.t()) {
            var result = me.setMULTIValue();
            if (result == null)
                me._win.hide();
            return true; //阻止窗口关闭，在save中根据返回结果处理
        } else if (btn === '取消'.t()) {
            me.isCanceling = true;
            return true;
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
            if (me.up("form"))
                entity = me.up("form").SIEView.getData();
            else
                entity = me.up("container").context.record;
            var selectItems = entity.getJoinEmployeeIds();
            if (selectItems != "" && selectItems != null) {
                selectItems.split(me.separator).forEach(function (item) {
                    if (item)
                        me._sourceViewSelectItems.push(item);
                });
            }
            me._createLayout(field);
        }
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
        });
        //可能移光标到 主控件
        me.on('focusleave',
            function (vthis, event, eOpts) {
                if (!event) return;

                if (me._win && event.toElement && me._win.owns(event.toElement) === false) {
                    //me._win.focus();
                    me._win.hide();
                    me._winNum = 0;
                }
            },
            me, {
            delay: 50
        });
        me._win.on('hide',
            function () {
                //console.log('关闭中-A-' + me.isCanceling);
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
                            if (me._sourceViewSelectItems.find(function (c) { return c == record.getId(); })) {
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




    setMULTIValue: function () {
        var me = this, displayVal2 = "", valueDisplay, list, key, mutiLinkfieldAndValueField, entity, binder, bindRec, i;
        me.mutiLinkField != "" && (valueDisplay = JSON.parse(me.mutiLinkField));
        me._targetSelectItems.items.forEach(function (model) {
            displayVal2 += me.separator + model.data[me.displayField]
        });
        displayVal2 = displayVal2.substring(me.separator.length);
        list = [];
        for (key in valueDisplay)
            mutiLinkfieldAndValueField = {},
                mutiLinkfieldAndValueField.linkField = key,
                mutiLinkfieldAndValueField.valueField = "",
                me._targetSelectItems.items.forEach(function (model) {
                    mutiLinkfieldAndValueField.valueField += me.separator + model.data[valueDisplay[key]]
                }),
                list.push(mutiLinkfieldAndValueField);
        for (entity = me.up("form") ? me.up("form").SIEView.getData() : me.up("container").context.record,
            binder = me.getBind(),
            binder && (bindRec = me._getBindRecord(),
                bindRec && bindRec.set(me.getName(), displayVal2)),
            i = 0; i < list.length; i++)
            entity.set(list[i].linkField, list[i].valueField.substring(me.separator.length));
        entity.setJoinEmployeeNames(displayVal2);
        me.checkChange();
        me.up("form") || me.up("container").context.view.refresh()
    },

});