Ext.define('SIE.Web.LES.MaterialPreparations.Scripts.WorkOrderModeEnumEditor', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.MpWorkOrderEnumEditor',
    listeners: {
        expand: function (field, eOpts) {
            this.getStore().filterBy(record => record.get('value') != 2);
        },
        // 下拉收起时清除过滤 
        collapse: function () {
            this.getStore().clearFilter();
        }
    }
});