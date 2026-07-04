Ext.define('SIE.Web.MES.TaskManagement.TaskConfigs.Editors.ByQtyEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.ByQtyEditor',
    layout: {
        type: 'hbox',
    },
    combineErrors: false,
    defaults: {
        hideLabel: true,
        margin: '0 5 0 0'
    },
    items: [{
        xtype: 'checkboxfield',
        name: 'ByQty',
        fieldLabel: 'Checkbox',
        boxLabel: '按照固定数量生成任务单',
        minValue: 0,
        width: 95,
        //allowBlank: false
    }, {
            name: 'Qty',
        xtype: 'numberfield',
        minValue: 0,
        width: 95,
        allowBlank: true
    }]
});