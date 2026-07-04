Ext.define('SIE.Web.MES.TaskManagement.SchedulingInfs.Scripts.SchedulingInfReportBehavior', {
    endIndex: null,

    beforeCreate: function (meta, curEntity) {
        var me = this;
        var gridConfig = meta.gridConfig;
        gridConfig.viewConfig = {
            getRowClass: function (record, index, rowParams, store) {
                if (record.data.IsImport == 0) {
                    return 'bg-danger';
                }
                if (record.data.IsImport == 1 && record.data.ImportQty < record.data.PlanQty) {
                    return 'bg-warning';
                }
            }
        };
    },

});