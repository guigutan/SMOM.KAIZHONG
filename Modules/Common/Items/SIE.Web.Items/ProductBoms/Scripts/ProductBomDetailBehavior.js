Ext.define('SIE.Web.Items.ProductBoms.ProductBomDetailBehavior', {
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getData();
        if (entity) {
            view.mon(view.getData(), 'propertyChanged', me.productBomDetailPropertyChanged, me);
        }
    },
    productBomDetailPropertyChanged: function (e) {
        if (e.property === "UnitQty") {
            if (e.value) {
                var precision = e.entity.getPrecision();
                if (precision != null) {
                    e.entity.setUnitQty(parseFloat(e.value.toFixed(precision)));
                }
            }
        }
    }
});
