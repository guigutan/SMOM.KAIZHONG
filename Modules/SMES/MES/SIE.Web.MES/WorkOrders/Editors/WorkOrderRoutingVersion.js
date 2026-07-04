Ext.define('SIE.Web.MES.WorkOrders.Editors.WorkOrderRoutingVersion', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.WorkOrderRoutingVersion',
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
        var woData = sieView.getCurrent().data;
        var rec = me._getContainerRecord();
        var searchValue = me.cbSearch.getRawValue();
        me._view.loadData({
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            filter: Ext.encode({ Method: 'GetRoutingVersion', Parameters: [woData.Type, woData.PlanBeginDate, woData.ProductId, woData.ResourceId, woData.ProcessSegmentId, woData.ProjectMaintainId] })
        });
        me._lastSearchValue = searchValue;
    },
});
