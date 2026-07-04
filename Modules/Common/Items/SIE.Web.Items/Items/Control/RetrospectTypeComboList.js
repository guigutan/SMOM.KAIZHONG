/**
 * 追溯方式绑定下拉事件
 */
Ext.define('SIE.Web.Items.Control.RetrospectTypeComboList', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.RetrospectTypeComboList',
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
        switch (entity.data.RetrospectType) {
            case 0:
                entity.setBatchRule(null);
                entity.setQty(1);
                break;
            default:
                break;
        }
    }
});