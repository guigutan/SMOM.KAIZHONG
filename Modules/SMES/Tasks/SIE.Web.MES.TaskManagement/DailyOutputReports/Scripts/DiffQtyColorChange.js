Ext.define('SIE.Web.MES.TaskManagement.DailyOutputReports.Scripts.DiffQtyColorChange', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.DiffQtyColorChange',
    endIndex: null,

    renderer: function (value, meta, cur) {
        if (value >= 0) {
            meta.tdStyle = 'background:green';
        } else if (value < 0) {
            meta.tdStyle = 'background:red';
        } 
        return value;
    }

});