SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.ReportRefreshCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "刷新", group: "edit", iconCls: "icon-Reload icon-green" },
    canExecute: function (listView) {
        return listView._parent.getCurrent() != null;
    },
    /*
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {
        var reportView = view;
        SIE.invokeDataQuery({
            method: 'GetOrCreateReportRecord',
            params: [reportView.getCurrent().data.DispatchTaskId],
            action: 'queryer',
            sync: true,
            type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result && res.Result.data && res.Result.data.items && res.Result.data.items[0]) {
                    reportView.setData(res.Result.data.items[0]);
                    var taskstatus = reportView.getParent().getCurrent().getTaskStatus();
                    if (taskstatus === 30)
                        SIE.Web.MES.TaskManagement.Reports.ReportCommon.getCommonModeInfo(reportView);
                }
            }
        });
    },
});