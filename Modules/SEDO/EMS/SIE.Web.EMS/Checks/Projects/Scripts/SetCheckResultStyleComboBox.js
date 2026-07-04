Ext.define('SIE.Web.EMS.Projects.Scripts.SetCheckResultStyleComboBox', {
    extend: 'SIE.grid.column.ComboBox',
    alias: 'widget.setCheckResultStyle_comboboxcolumn',
    renderer: function (value, meta) {
        if (value == 0) {
            meta.tdCls = "icon-red";
            return "不合格".t();
        }
        else if (value == 1)
            return "合格".t();
        else if (value == 2)
            return "不适用".t();
    }
});