Ext.define('SIE.Web.Tech.Processs.Editors.ProcessConditionEditor', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.ProcessConditionEditor',
    matchFieldWidth: false,
    _win: null,
    createPicker: function () {
        var me = this;
        var field = me.up().field;
        me._win = Ext.create('SIE.Web.Tech.Processs.Controls.ProcessConditionDialog', {
            title: '编辑脚本'.t(),
            bindcontent: '{field.value}',
            callback: function (btn) {
                if (btn === Ext.window.MessageBox.OK) {
                    field.setValue(field.value);
                    me.collapse();
                } else if (btn === Ext.window.MessageBox.CANCEL) {
                    me.collapse();
                }
            }
        });
        me._win.getViewModel().setData({
            field: field
        });
        return me._win;
    }
});
