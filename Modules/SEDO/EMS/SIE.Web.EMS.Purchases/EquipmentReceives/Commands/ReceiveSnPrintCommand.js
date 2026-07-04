SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentReceives.Commands.ReceiveSnPrintCommand', {
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Purchases.EquipmentReceives.ReceiveSnPrintViewModel",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.createDetailView(res);
                var entity = new detailView._model();
                detailView.setData(entity);
                var win = SIE.Window.show({
                    title: "序列号打印".t(),
                    width: 480,
                    height: 200,
                    items: detailView.getControl(),
                    id: "ReceiveSnPrintCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var reprintInfo = detailView.getData().data;
                            if (!reprintInfo.TemplateId || reprintInfo.TemplateId <= 0) {
                                SIE.Msg.showMessage("打印模板不能为空".L10N());
                                return false;
                            }
                            me.print(view, reprintInfo, win);
                            return false;
                        }
                    }
                });
            }
        });
    },
    print: function (view, reprintInfo, win) {
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer",
            method: "ReceiveSnPrint",
            params: [view.getSelectionIds(), reprintInfo],
            async: false,
            token: view.token,
            success: function (resu) {
                var rstPrint = resu.Result;
                if (rstPrint.ErrMsg !== '') {
                    SIE.Msg.showError(rstPrint.ErrMsg);
                    return false;
                } else {
                    win.close();
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
                                CRT.Workbench.showPageDialog({ id: 'ReceiveSnPrint_rpt', text: "序列号打印".t(), method: 'POST', url: printUrl, params: param });
                        }
                    }
                    view.reloadData();
                }
            }
        });
    }
});