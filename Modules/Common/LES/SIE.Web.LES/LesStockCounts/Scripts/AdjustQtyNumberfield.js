Ext.define('SIE.Web.LES.Control.AdjustQtyNumberfield', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.AdjustQtyNumberfield',
    beforeValue: null,
    listeners: {
        change: function (comp, newValue, oldValue) {
            var me = this;
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record
            }
            entity.setDiffQty(newValue - entity.getQty());
        },
    },
});