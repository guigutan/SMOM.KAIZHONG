Ext.define('SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessLookUp', {
    extend: 'SIE.control.PagingLookUpMethod',
    alias: 'widget.RoutingProcessLookUp',
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
        var id = this._getSIEView().getParent().getCurrent().data.Id;
        var versionId=this._getSIEView().getParent().getCurrent().data.RoutingVersionId;
        me._view.setData(null);
        
        me._view.loadData({
            action: 'queryer',
            type: 'SIE.Web.MES.Routings.RoutingBoms.DataQueryers.RoutingBomDetailDataQuery',
            filter: Ext.encode({ Method: 'GetRoutingBomProcesses', Parameters: [id, versionId] })
        });
        me._lastSearchValue = searchValue;
    },
});