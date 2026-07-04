Ext.define('SIE.Web.Items.Items.ItemCategoryRelationComboList', {
    extend: 'SIE.control.ComboLink',
    alias: 'widget.itemCategoryRelationComboList',
    _searchByDSP: function (pageNum) {
        var me = this,
            dsp = this.dataSourceProperty;
        var filter = {};
        var rec = me.up().context.grid.SIEView.getParent().getCurrent();

        filter = {
            Parameters: {
                EntityType: me.up().context.grid.SIEView.getParent().model,
                Entity: rec.data,
                DataSourceProperty: dsp
            }
        };
        var searchValue = me.cbSearch.getRawValue();
        me._view.loadData({
            action: 'lookup',
            filter: SIE.data.Utils.seriaizeRequest(filter),
            searchKeyWord: (searchValue ? '%' + searchValue + '%' : ''),
            page: pageNum,
            criteria: null
        });
        me._lastSearchValue = searchValue;
    },
});