//工单属性页签属性编辑器
Ext.define('SIE.Web.MES.WorkOrders.Editors.WoProvalueDefinition', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.WoProvalueDefinition',
    triggerCls: "x-form-arrow-trigger",
    _onSearchBoxTriggerClick: function (pageNum) {
        pageNum = pageNum || 1;
        var me = this;
        me._searchByItem(pageNum);
    },
    _searchByItem: function (pageNum) {
        if (typeof (_isQuerySelectItems) === "undefined")
            _isQuerySelectItems = this._isQuerySelectItems;
        var me = this,
            dsp = this.dataSourceProperty;

        var sieView = me._getSIEView();
        //console.log(sieView);
        if (!sieView) {
            me._searchByRawValue();
            return;
            //viewGroup = view.viewGroup;
        }
        var filter = {};
        var itemid = 0;
        if (sieView._parent.label == 'BOM信息') {
            itemid = sieView._parent.getCurrent().data.ItemId;
        }
        else if (sieView._parent.label == '工单') {
            itemid = sieView._parent.getData().data.ProductId;
        }
        else if (sieView._parent.label == '工序BOM') {
            itemid = sieView._parent.getCurrent().data.ItemId;
        }
        else if (sieView._parent.label == '配送管理') {
            itemid = sieView._parent.getCurrent().data.ItemId;
        }
        var rec = me._getContainerRecord();
        var searchValue = me.cbSearch.getRawValue();
        me._view.loadData({
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            filter: Ext.encode({ Method: 'GetDefinition', Parameters: [itemid, pageNum, me.pageSize, (searchValue ? '%' + searchValue + '%' : '')] })
        });
        me._lastSearchValue = searchValue;
    },
});
//工单属性页签属性值编辑器
Ext.define('SIE.Web.MES.WorkOrders.Editors.WoProvalueDefinitionMulti', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.WoProvalueDefinitionMulti',
    triggerCls: "x-form-arrow-trigger",
    initComponent: function () {
        var me = this;
        me.callParent();

        if (!me.bindDisplayField) {
            me.bindDisplayField = me.getName() + "_Display";
        }
        me.multiSelect = "Multi";
    },

    //_onFocusLeaveIntegrate会触发setMULTIValues，再调用当前的setMULTIValue
    setMULTIValue: function () {
        var me = this;
        me.value = me._targetSelectItems.keys;
        var displayVal2 = "";
        var vals = "";
        var ids = "";
        var displayField = me.displayField;
        var definedId = me.up('container').context.record.data.DefinitionId;
        var items = me._targetSelectItems.items.where(function (p) { return p.data.DefinitionId == definedId; });
        items.forEach(function (model) {
            displayVal2 += "," + model.data[displayField];
            vals += model.data.Value + ",";
            ids += model.data.Id + ",";
        });
        displayVal2 = displayVal2.substring(1);
        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        entity.data[me.getName()] = ids;
        entity.data.Values = items.select(function (p) { return p.data.Value; });
        entity.data[me.bindDisplayField] = displayVal2;
        me.setRawValue(displayVal2);
        me.lastSelectionRecord = {
            value: entity.data[me.getName()],
            rawValue: displayVal2
        };
        me.checkChange();
    },
    //_onSearchBoxTriggerClick: function (pageNum) {
    //    pageNum = pageNum || 1;
    //    var me = this;
    //    me._searchByItem(pageNum);
    //},
    //_searchByItem: function (pageNum) {
    //    var me = this,
    //        dsp = this.dataSourceProperty;
    //    me._lastSearchValue = '2,3';
    //}
});
//工单工序BOM属性编辑器
Ext.define('SIE.Web.MES.WorkOrders.Editors.WoProcessBomDefinition', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.WoProcessBomDefinition',
    triggerCls: "x-form-arrow-trigger",


    _onSearchBoxTriggerClick: function (pageNum) {
        pageNum = pageNum || 1;
        var me = this;
        me._searchByItem(pageNum);
    },
    _isQuerySelectItems: true,
    catalogReloadData: true,
    _searchByItem: function (pageNum) {
        //继承时发现_isQuerySelectItems偶尔会未定义，而基类又会直接使用_isQuerySelectItems，使得报错。所以这里再定义一次。
        if (typeof (_isQuerySelectItems) === "undefined")
            _isQuerySelectItems = this._isQuerySelectItems;
        var me = this,
            dsp = this.dataSourceProperty;

        var sieView = me._getSIEView();
        //console.log(sieView);
        if (!sieView) {
            me._searchByRawValue();
            return;
            //viewGroup = view.viewGroup;
        }
        var filter = {};

        var rec = me._getContainerRecord();
        var searchValue = me.cbSearch.getRawValue();
        me._view.loadData({
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            filter: Ext.encode({ Method: 'GetDefinition', Parameters: [sieView._parent.getCurrent().getData().ItemId, pageNum, me.pageSize, (searchValue ? '%' + searchValue + '%' : '')] })
        });
        me._lastSearchValue = searchValue;
    },
});

Ext.define('SIE.Web.MES.WorkOrders.Editors.WoProcessBomDefinitionMulti', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.WoProcessBomDefinitionMulti',
    triggerCls: "x-form-arrow-trigger",
    //_onFocusLeaveIntegrate会触发setMULTIValues，再调用当前的setMULTIValue
    setMULTIValue: function () {
        var me = this;
        me.value = me._targetSelectItems.keys;
        var displayVal2 = "";
        var vals = "";
        var ids = "";
        var displayField = me.displayField;
        var definedId = me.up('container').context.record.data.DefinitionId;
        var items = me._targetSelectItems.items.where(function (p) { return p.data.DefinitionId == definedId; });
        items.forEach(function (model) {
            displayVal2 += "," + model.data[displayField];
            vals += model.data.Value + ",";
            ids += model.data.Id + ",";
        });
        displayVal2 = displayVal2.substring(1);
        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        entity.data[me.getName()] = ids;
        entity.data.Values = items.select(function (p) { return p.data.Value; });
        entity.data[me.bindDisplayField] = displayVal2;
        me.setRawValue(displayVal2);
        me.lastSelectionRecord = {
            value: entity.data[me.getName()],
            rawValue: displayVal2
        };
        me.checkChange();
    },

});


Ext.define('SIE.Web.MES.WorkOrders.Editors.WorkOrderProvalueCombolist', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.workorderprovaluecombolist',
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
        var displayVal2 = "";
        //获取关联行的键值对
        //示例{"colspan1":"code","colspan2":"name","colspan3":"email"}
        //var valueDisplay = JSON.parse(me.mutiLinkField);
        //获取当前选择框的展示值
        var displayId = '';
        var valuesArray = [];
        me._targetSelectItems.items.forEach(function (model) {
            displayVal2 += me.separator + model.data[me.displayField];
            displayId += me.separator + model.data["Id"];
            valuesArray.push(model.data[me.displayField]);
        });
        displayVal2 = displayVal2.substring(me.separator.length);
        displayId = displayId.substring(me.separator.length);
        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        entity.data["DisplayValues"] = displayVal2;
        entity.data["ItemValue"] = displayVal2;
        entity.data["Value"] = displayVal2;
        entity.data["Values"] = valuesArray;
        entity.data["DefinitionValueId"] = displayVal2;
        me.setRawValue(displayVal2);
        me.checkChange();
        me.dirty = true;
        if (!me.up("form")) {
            var mainview = null;
            if (me.up('gridpanel').SIEView._parent.label != '工单') {
                mainview = me.up('gridpanel').SIEView._parent._parent;
            }
            else {
                mainview = me.up('gridpanel').SIEView._parent;
            }
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
        if (entity.data.Values)
            entity.data["DefinitionValueId"] = entity.data.Values.join(',');
    },
    onpopupWinbtn: function (btn) {
        var me = this;
        if (btn === '确定'.t()) {
            var entity;
            me.setMULTIValue();
            //if (!me.up("form"))
            //    entity = me.up("container").context.record;
            //else
            //    entity = me.up("form").SIEView.getData();
            //if (entity.getDefectDescription() === "")
            //    entity.setDefectDescription(entity.getDisplayDefectDescription());
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
                var itemid = me.up('container').context.record.data.ItemId;
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