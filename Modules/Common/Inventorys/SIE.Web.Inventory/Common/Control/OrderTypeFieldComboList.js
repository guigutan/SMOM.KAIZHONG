Ext.define('SIE.Web.Inventory.Common.Control.OrderTypeFieldComboList', {
    extend: 'SIE.control.XComboBox',
    alias: 'widget.ordertypefieldcombolist',
    listeners: {
        select: function (combo, record, index) {
            var me = this;
            entity = combo.up().SIEView.getData();
            me.onSortFieldChanged(entity, record);
        }
    },
    onSortFieldChanged: function (entity, record) {
        var data = record.data;
        if (data.value == null) {
            entity.setTransactionId(null);
            entity.setTransactionId_Display(null);
        }
    },
    initComponent: function () {
        var me = this;
        if (me.up("form")) {
            var view = me.up("form").SIEView;
        }
        var item = me.store.data.first(function (f) { return f.value == 80 });
        me.store.data.remove(item);
        me.callParent(arguments);
    },

});