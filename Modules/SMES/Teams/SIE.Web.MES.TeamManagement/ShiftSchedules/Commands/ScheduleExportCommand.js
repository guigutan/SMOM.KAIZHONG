SIE.defineCommand('SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleExportCommand', {
    meta: { text: "导出Excel", group: "business", iconCls: "icon-ExportData icon-blue" },
    execute: function (view) {
        var me = this;
        var record = view._relations[0]._target.getCurrent();
        var criteria = record.data;
        if (!me.validateCriteria(criteria))
            return;
        SIE.invokeDataQuery({
            method: 'ExportShiftSchedule',
            params: [criteria],
            action: 'queryer',
            type: 'SIE.Web.MES.TeamManagement.ShiftSchedules.ShiftScheduleDataQueryer',
            token: view.getToken(),
            success: function (res) {
                if (res.Success) {
                    var exportData = res.Result;
                    if (exportData && exportData.Tables && exportData.Tables.length === 0) {
                        me.timer = Ext.defer(function () {
                            me.timer = null;
                            Ext.MessageBox.hide();
                        }, 1000);
                        SIE.Msg.showMessage("没有可导出的数据".L10N());
                    }
                    else {
                        me.generateExcel(exportData);
                        me.timer = Ext.defer(function () {
                            me.timer = null;
                            Ext.MessageBox.hide();
                        }, 1000);
                    }
                }
            }
        });
    },

    generateExcel: function (exportData) {
        SIE.Web.MES.Common.Scripts.Helpers.ExportExcelHelper.tablesToMultiSheetExcel(exportData, '排班表'.L10N() + Ext.util.Format.date(new Date(), 'Ymdhis'), false);
    },

    validateCriteria: function (criteria) {
        if (criteria == null)
            return false;
        if (criteria.ScheduleDate.BeginValue == null || criteria.ScheduleDate.EndValue == null) {
            SIE.Msg.showMessage('开始日期不能为空');
            return false;
        }
        if (criteria.ScheduleDate.BeginValue > criteria.ScheduleDate.EndValue)
            return false;
        return true;
    }
});

