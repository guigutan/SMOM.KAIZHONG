Ext.define('SIE.Web.Warehouses.Control.HumidityTypeComboList', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.HumidityTypeComboList',
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
        switch (entity.data.HumidityType) {
            case SIE.Warehouses.HumidityType.Normal.value:
                entity.setHumidityLower(0);
                entity.setHumidityUpper(95);
                break;
            case SIE.Warehouses.HumidityType.Low.value:
                entity.setHumidityLower(40);
                entity.setHumidityUpper(60);
                break;
            case SIE.Warehouses.HumidityType.Dry.value:
                entity.setHumidityLower(0);
                entity.setHumidityUpper(10);
                break;
            default:
                entity.setHumidityLower(null);
                entity.setHumidityUpper(null);
                break;
        }
    }
});