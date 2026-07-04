Ext.define('SIE.Web.MES.TaskManagement.Completion.Scripts.CapacityLoadColorChange', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.CapacityLoadColorChange',
    endIndex: null,

    renderer: function (value, meta, cur) {
        if (cur.getCapacityLoad() > 100) {
            meta.tdStyle = 'background:red';
            return value + '%';
        } else if (cur.getCapacityLoad() > 0) {
            meta.tdStyle = 'background:green';
            return value + '%';
        } 
        return value;
    }

});