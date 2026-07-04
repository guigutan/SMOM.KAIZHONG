Ext.define('SIE.Web.MES.TaskManagement.Reports.Scripts.ReportWipBatchBatchColumnColor', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.setReportWipBatchBatchColumnColorStyle',
    renderer: function (value, meta) {
        if (meta.record.data.Color == '1')
            meta.tdStyle = "background:green";
        return value;
    },
});