Ext.define('SIE.Web.MES.TeamManagement.ClockingIns.Scripts.SetClockInStateStyle', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.SetClockInStateStyle',
    renderer: function (value, meta) {
        if (value == 1) {
            meta.tdStyle = "color:red;";
            return "异常".t();
        }
        else if (value == 0)
        {
            meta.tdStyle = "color:green;";
            return "正常".t();
        }           
        else if (value == 2) {           
            return "休息".t();
        }
    }
});