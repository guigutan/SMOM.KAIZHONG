SIE.defineCommand('SIE.Web.DataTrace.TraceMainDatas.Commands.ViewMutiDocumenttCommand', {
    meta: { text: "文档预览", group: "business", iconCls: "iconfont icon-PrintData icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {
        var item = view.getCurrent();
        if (item != null) {
            return true;
        }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var idsArr = view.getSelectionIds();
        Ext.MessageBox.show({
            msg: '加载打印数据中, 请稍等...'.L10N(),
            progressText: '加载中...'.L10N(),
            width: 300,
            closable: false,
            //modal: true,
            wait: {
                interval: 200
            }
        });
        view.execute({
            data: { data: Ext.encode(idsArr) },
            success: function (res) {
                var params = {};
                var paths = [];
                res.Result.forEach(function (printTemp) {
                    var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({ ReportType: printTemp.Type, ReportData: printTemp });
                    var cfg = printCmpt.getExtTarget();
                    if (cfg && cfg.printCallback) {
                        cfg.printCallback(printCmpt);
                    }
                    else {
                        var param = printCmpt.getPrintParams();
                        if (!printCmpt.hasError()) {
                            paths.push(param.path);
                            //if (!printCmpt.hasError())
                            //    CRT.Workbench.showPageDialog({ id: 'userqrcode_rpt', text: "电子批文档预览".t(), method: 'POST', url: printUrl, params: param });
                        }
                    }
                });
                params.path = paths.join();
                me.operatePdfFile(params);
            }
        });
    },
    operatePdfFile: function (params) {
        CRT.Workbench.showPageDialog({ id: 'userqrcode_rpt', text: "电子批文档预览".t(), method: 'POST', url: "/SimpleList/Reports/MultReports", params: params });
        Ext.MessageBox.close();
    }
});
