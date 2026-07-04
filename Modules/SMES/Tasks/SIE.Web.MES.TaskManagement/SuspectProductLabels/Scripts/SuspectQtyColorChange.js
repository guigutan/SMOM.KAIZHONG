Ext.define('SIE.Web.MES.TaskManagement.SuspectProductLabels.Scripts.SuspectQtyColorChange', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.SuspectQtyColorChange',
    endIndex: null,

    renderer: function (value, meta, cur) {
        if (cur.getRemainingQry() > 0) {
            meta.tdStyle = 'background:red';
        }
        return value;
    }

});