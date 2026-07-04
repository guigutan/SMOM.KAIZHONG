SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.PrintStoreDetailLabelCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "标签补打", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        if (view.hasSelectedEntities()) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {

                if (item.getControlMethod() != view.getSelection()[0].data.ControlMethod || view.getSelection()[0].data.ControlMethod == 10) {
                    flag = false;
                    return false;
                }

                if (Ext.isEmpty(item.getBatchNumber()) && view.getSelection()[0].data.ControlMethod == 20) {
                    flag = false;
                    return false;
                }

                if (Ext.isEmpty(item.getSn()) && view.getSelection()[0].data.ControlMethod == 30) {
                    flag = false;
                    return false;
                }
            });
            return flag;
        }
        else {
            return false;
        }
    },
    execute: function (view, source) {

        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer",
            method: "LabelPrint",
            params: [view.getSelectionIds(), view.getSelection()[0].data.ControlMethod],
            async: false,
            token: view.token,
            success: function (resu) {
                var rstPrint = resu.Result;
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
                                CRT.Workbench.showPageDialog({ id: 'StoreDetailLabelPrint_rpt', text: "打印".t(), method: 'POST', url: printUrl, params: param });
                        }
                    }
                }
            }
        });
    },
});