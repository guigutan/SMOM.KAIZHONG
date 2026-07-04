Ext.define('SIE.Web.Fixtures.Demands.Editors.EncodeEditor', {
    extend: 'SIE.control.PagingLookUpMethod',
    alias: 'widget.EncodeEditor',
    setValue: function (value) {
        var me = this;
        this.defaultSetValue(value);
    },
    /**
    * 请求查询(lookup)
    * @param {type} pageNum
    */
    _searchByDSP: function (pageNum) {
        var me = this.control;
        var rec = this._getContainerRecord();
        var searchValue = me.cbSearch.getRawValue();
        var demand = this._getSIEView().getCurrent().data;
        var parentDemand = this._getSIEView().getParent().getCurrent().data;
        var woId = parentDemand.WorkOrderId;
        var processStegmentId = parentDemand.ProcessSegmentId;
        me._view.setData(null);
        me._view.loadData({
            action: 'queryer',
            type: 'SIE.Web.Fixtures.Demands.DataQuery.FixtureDemandDataQueryer',
            filter: Ext.encode({
                Method: 'GetFixtureEncodeList', Parameters: [woId, demand.FixtureTypeId, demand.FixtureModelId,
                    processStegmentId,pageNum, me.pageSize, (searchValue ?  searchValue  : '')]
            })
        });
        me._lastSearchValue = searchValue;
    },
});