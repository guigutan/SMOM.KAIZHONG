Ext.define('SIE.Web.MES.LoadItems.DeductItems.Scripts.WoCostItemEnumEditor', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.woCostItemEnumEditor',
    listeners: {
        expand: function (field, eOpts) {
            this.getStore().filterBy(record => record.get('value') != 30);
        },
        // 下拉收起时清除过滤 
        collapse: function () {
            this.getStore().clearFilter();
        }
    }
});