Ext.define('SIE.Web.EMS.Equipments.AlarmStates.Scripts.SetAlarmStateStyleComboBox', {
    extend: 'SIE.grid.column.ComboBox',
    alias: 'widget.setAlarmStateStyle_comboboxcolumn',
    renderer: function (value, meta) {
        if (value == 1) {
            meta.tdCls = "icon-red";
            return "开启".t();
        } else if (value == 2) {
            return "关闭".t();
        } else if (value == 3) {
            return "无效".t();
        }
    }
});