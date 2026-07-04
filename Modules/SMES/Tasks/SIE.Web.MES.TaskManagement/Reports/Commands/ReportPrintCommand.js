SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.ReportPrintCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "报工并打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,
    canExecute: function (view) {
        return view._parent.getSelection().length == 1 && view._parent._current.data.TaskStatus == 30 && view._parent._current.data.ReportMode == 1;
    },
    /*
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'TryGetReportPrintemplate',
            params: [],
            async: false,
            action: 'queryer',
            type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result == true) {
                    var recordId = SIE.Web.MES.TaskManagement.Reports.ReportCommon.saveOrSubmmitReport(view, true, me.meta.command);

                    SIE.invokeDataQuery({
                        method: 'ReportLabelPrint',
                        params: [recordId],
                        async: false,
                        action: 'queryer',
                        type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
                        token: view.token,
                        success: function (res) {

                            var rstPrint = res.Result;
                            if (rstPrint.ErrMsg !== '') {
                                SIE.Msg.showError(rstPrint.ErrMsg);
                                return false;
                            } else {
                                var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({ ReportType: rstPrint.Type, ReportData: { path: rstPrint.Url, content: rstPrint.Url } });
                                var cfg = printCmpt.getExtTarget();
                                if (cfg && cfg.printCallback) {
                                    cfg.printCallback(printCmpt);
                                }
                                else {
                                    var param = printCmpt.getPrintParams();
                                    if (!printCmpt.hasError()) {
                                        var printUrl = printCmpt.getPrintUrl();
                                        if (!printCmpt.hasError())
                                            CRT.Workbench.showPageDialog({ id: 'ReportLabelPrint_rpt', text: "打印".t(), method: 'POST', url: printUrl, params: param });
                                    }
                                }
                            }
                        }
                    });
                }
            }
        });
    },
});