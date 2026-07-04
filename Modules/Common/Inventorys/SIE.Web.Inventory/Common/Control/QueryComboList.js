Ext.define('SIE.Web.Inventory.Common.Control.QueryComboList', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.querycombolist',
    triggerCls: "x-form-arrow-trigger",
    onExpand: function () {
        //console.log('onExpand');
        var me = this;
        me.callParent();
        var tableView = me.grid.getView();
        if (!me.up("form")) {
            me.cbSearch.setRawValue("");
            me._isQuerySelectItems = true;
            me._onSearchBoxTriggerClick();
        }
        else {
            me._onSearchBoxTriggerClick();
        }
        tableView.refresh();
    },
    _onSearchBoxTriggerClick: function (pageNum) {
        pageNum = pageNum || 1;
        var me = this;

        if (me.queryMode == 'remote') {
            me._searchByDSP(pageNum);
        }
        else {
            me.doLocalQuery();
        }
    },
    _searchByDSP: function (pageNum) {
        //继承时发现_isQuerySelectItems偶尔会未定义，而基类又会直接使用_isQuerySelectItems，使得报错。所以这里再定义一次。
        //if (typeof (_isQuerySelectItems) === "undefined")
        //    _isQuerySelectItems = this._isQuerySelectItems;
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
        var tempView;
        if (me.up("form"))
            tempView = me.up("form").SIEView;
        else
            tempView = me.up("container").context;
        var parent = tempView.associateCmd.view.getParent().getCurrent();
        var searchValue = me.cbSearch.getRawValue();
        this._loadDataQueryer(me, parent, pageNum, searchValue);
        me._lastSearchValue = searchValue;
    },
    _loadDataQueryer: function (me, parent, pageNum, searchValue) {
        //me._view.loadData({
        //    action: 'queryer',
        //    type: 'SIE.Web.WMS.Shipment.DataQueryer.OnHandItemDataQueryer',
        //    filter: Ext.encode({ Method: 'GetOnHandItem', Parameters: [parent.getData().ShippingWareHouseId, pageNum, me.pageSize, (searchValue ? '%' + searchValue + '%' : '')] })
        //});
    }
});