Ext.define('SIE.Web.MES.WorkOrders.Editors.ProcessBomRoutingProcess', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.ProcessBomRoutingProcess',
    triggerCls: "x-form-arrow-trigger",
    _onSearchBoxTriggerClick: function (pageNum) {
        pageNum = pageNum || 1;
        var me = this;
        me._searchByItem(pageNum);

    },
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
        var woData = sieView._parent.getData().data;
        var rec = me._getContainerRecord();
        var searchValue = me.cbSearch.getRawValue();
        var routingProcess = sieView._parent._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderRoutingProcess'; });
        var routData = routingProcess.getData().data.items.where(function (p) { return p.data.ProcessType == 15 || p.data.ProcessType == 25 || p.data.ProcessType == 13; });  //装配、批次装配、返工
        me._view.getData().data.clear();
        var store = me._view.getControl().getStore();
        me._view.getControl().setStore(store);
        me._view.getControl().getStore().data.add(routData)
        me._lastSearchValue = searchValue;
    },
});

Ext.define('SIE.Web.MES.WorkOrders.Editors.ProcessBomItem', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.ProcessBomItem',
    triggerCls: "x-form-arrow-trigger",
    _onRowdblClick: function (vthis, record, element, rowIndex, e, eOpts) {
        var me = this;
        if (record) {
            me._SelectItems = [];
            me.setValue(record);
            me._SelectItems.push(record);
            me._win.hide();
            var data = e.view.up().ownerCt.up().up().up().ownerCt.SIEView.getCurrent();
            data.setUnitId(record.data.UnitId);
            data.setUnitId_Display(record.data.UnitId_Display);
            data.setUnitName(record.data.UnitName);
        }
    },
});
