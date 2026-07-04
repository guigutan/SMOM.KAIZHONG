/**
 * 批次规则绑定下拉事件
 */
Ext.define('SIE.Web.Items.Control.BatchRuleComboList', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.BatchRuleComboList',
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
        if (entity.data.BatchRule != 0) {
            entity.setQty(1);
        }
    }
});