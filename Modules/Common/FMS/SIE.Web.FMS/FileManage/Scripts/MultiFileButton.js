Ext.define('Ext.form.field.MultiFileButton', {
    extend: 'Ext.form.field.FileButton',
    alias: 'widget.filebutton',
    afterRender: function () {
        var me = this;            
        me.callParent(arguments);
        var fileDom = me.getEl().down('input[type=file]');
        fileDom.dom.setAttribute("multiple", "multiple");
    }
});