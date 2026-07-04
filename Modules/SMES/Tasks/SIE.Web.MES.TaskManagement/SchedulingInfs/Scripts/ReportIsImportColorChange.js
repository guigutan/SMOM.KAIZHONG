Ext.define('SIE.Web.MES.TaskManagement.SchedulingInfs.Scripts.ReportIsImportColorChange', {
    extend: 'Ext.grid.column.Column',
    alias:'widget.ReportIsImportColorChange',
    endIndex: null,

    renderer: function (value, meta, cur) {
        if (cur.getIsImport() == 0) {
            meta.tdStyle = 'background:red';
            return "否";
        }
        if (cur.getIsImport() == 1 && cur.getImportQty() < cur.getPlanQty())
        {
            meta.tdStyle = 'background:yellow';
            return "是";
        }

        if (cur.getIsImport() == 0)
            return "否";
        if (cur.getIsImport() == 1)
            return "是";
    }

});