Ext.define('SIE.Web.Warehouses.Control.TemperatureTypeComboList', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.TemperatureTypeComboList',
    listeners: {
        select: function (combo, record, index) {
            var me = this;
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record
            }
            if (entity != null) {
                me.onTypeChanged(entity);
            }
        }
    },
    onTypeChanged: function (entity) {
        switch (entity.data.TemperatureType) {
            case SIE.Warehouses.TemperatureType.Normal.value:
                entity.setTemperatureLower(0);
                entity.setTemperatureUpper(30);
                break;
            case SIE.Warehouses.TemperatureType.Low.value:
                entity.setTemperatureLower(15);
                entity.setTemperatureUpper(25);
                break;
            case SIE.Warehouses.TemperatureType.Cold.value:
                entity.setTemperatureLower(0);
                entity.setTemperatureUpper(10);
                break;
            case SIE.Warehouses.TemperatureType.Freezing.value:
                entity.setTemperatureLower(-24);
                entity.setTemperatureUpper(-4);
                break;
            default:
                entity.setTemperatureLower(null);
                entity.setTemperatureUpper(null);
                break;
        }
    }
});