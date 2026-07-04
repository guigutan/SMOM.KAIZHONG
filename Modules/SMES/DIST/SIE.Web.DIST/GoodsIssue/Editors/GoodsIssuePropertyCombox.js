
Ext.define('SIE.Web.DIST.Editors.GoodsIssuePropertyValueCb', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.goodsissuepropertyvaluecb',
    /**
    * 确定事件
    * @param btn--
    * @returns
    */initComponent: function () {
        var me = this;
        me.callParent();
        me.multiSelect = 'MULTI';
        me.separator = ',';
    },
    setMULTIValue: function () {
        var me = this;
        var displayVal = "";
        var valuesArray = [];
        me._targetSelectItems.items.forEach(function (model) {
            displayVal += me.separator + model.data[me.displayField];
            valuesArray.push(model.data[me.displayField]);
        });
        displayVal = displayVal.substring(me.separator.length);
        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        entity.data["DisplayValues"] = displayVal;
        entity.data["ItemValue"] = displayVal;
        entity.data["Value"] = displayVal;
        entity.data["Values"] = valuesArray;
        entity.data["DefinitionValueId"] = displayVal;
        me.setRawValue(displayVal);
        me.checkChange();
        me.dirty = true;
        if (!me.up("form")) {
            var mainview = me.up('gridpanel').SIEView._parent;
            me.up("container").context.view.refresh();
            mainview.getData().dirty = true;
            mainview.syncCmdState(mainview, true);
        }
    },
    cancelSetValue: function () {
        var entity;
        var me = this;
        if (!me.up("form"))

            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        entity.data["DefinitionValueId"] = entity.data["ItemValue"];
    },
    onpopupWinbtn: function (btn) {
        var me = this;
        if (btn === '确定'.t()) {
            var entity;
            me.setMULTIValue();
            me._win.hide();
            return true; //阻止窗口关闭，在save中根据返回结果处理
        } else if (btn === '取消'.t()) {
            me.cancelSetValue();
            me.isCanceling = true;
            return true;
        }
    },
    _createLayout: function (field) {
        var me = this;
        if (!me.model)
            SIE.Msg.showWarning('请设置数据关联实体'.t());
        SIE.AutoUI.getMeta({
            model: me.model,
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            callback: function (blocks) {
                me._queryBlockProcess(blocks);
                me._gridBlockProcess(blocks);
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                //var entity = new SIE.Items.ItemPropertyValue();
                var entity = SIE.data.Utils.createStore({
                    model: 'SIE.Items.ItemPropertyValue',
                    remoteSort: true
                });
                ui._view.setData(entity);
                me._popupWin(ui, me.inputEl);
                // me._reloadTargetViewData();
                var defiid = me.up('container').context.record.data.DefinitionId;
                var itemid = me.up('container').context.grid.SIEView._parent.getData().data.ItemId;
                ui._view.loadData({
                    action: 'queryer',
                    type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
                    filter: Ext.encode({ Method: 'GetDefinitionValues', Parameters: [itemid, defiid, ''] })
                });
                me._layouted = true;
            }
        });
    },

});