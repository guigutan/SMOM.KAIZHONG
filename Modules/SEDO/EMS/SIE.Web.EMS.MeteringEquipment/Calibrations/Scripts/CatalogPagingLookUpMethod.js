
Ext.define('SIE.Web.EMS.MeteringEquipment.Calibrations.Scripts.CatalogPagingLookUpMethod', {
    extend: 'SIE.control.PagingLookUpMethod',
    /**
     * 生成请求参数据
     * @param {any} dsp
     */
    _searchByDSPfilter: function (dsp) {
        var me = this.control;
        var filter = {
            Parameters: {
                EntityType: this._getSIEView().model,
                Entity: this._getContainerRecord().data,
                DataSourceProperty: dsp
            }
        };

        return filter;
    },
});

