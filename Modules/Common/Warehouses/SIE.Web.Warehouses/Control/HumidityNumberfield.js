Ext.define('SIE.Web.Warehouses.Control.HumidityNumberfield', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.HumidityNumberfield',
    beforeValue: null,
    listeners: {
        focus: function () {
            //获取焦点
            var me = this;
            beforeValue = me.value;
        },
        blur: function () {
            //失去焦点事件
            var me = this;
            if (me.value != beforeValue) {
                if (me.up("form")) {
                    entity = me.up("form").SIEView.getData();
                } else {
                    entity = me.up('container').context.record
                }
                if (entity != null)
                    entity.setHumidityType(SIE.Warehouses.HumidityType.Custom.value);
            }
        },
        spinend: function (field, eOpts) {
            var me = this;
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record
            }
            if (entity != null)
                entity.setHumidityType(SIE.Warehouses.HumidityType.Custom.value);
        }
    },
});