Ext.define('SIE.Web.Warehouses.Scripts.StationGroupWarehouseComboList', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.StationGroupWarehouseComboList',
    listeners: {
        change: function (field, newValue, oldValue, eOpts) {
            var me = this;
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record
            }

            if (entity != null) {
                entity.setStorageAreaId(null);
                entity.setStorageAreaId_Display(null);
            }
        }
    },
});