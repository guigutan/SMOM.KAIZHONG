SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.ReportTaskRefreshCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "刷新", group: "edit", iconCls: "icon-Reload icon-green" },
    canExecute: function (listView) {
        return true;
    },
    /*
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {
        var view = CRT.Context.PageContext.getQueryView();
        if (view) view.tryExecuteQuery();
    },
});