SIE.defineCommand('SIE.Web.Barcodes.OutBoxReprintCommand', {
    meta: { text: "外标签补打", group: "edit", iconCls: "icon-Package icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        var first = selectModels[0].data.WorkOrderId;
        SIE.each(selectModels, function (model) {
            if (model.data.WorkOrderId != first) {
                res = false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var barcodeId = view.getCurrent().data.Id;
        var me = this;
        SIE.AutoUI.getMeta({
            model: "SIE.Web.Barcodes.Barcodes.ViewModels.ReprintInfo",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var mainBolck;
                if (res.mainBolck)
                    mainBolck = res.mainBolck;
                else
                    mainBolck = res;
                var detailView = SIE.AutoUI.createDetailView(mainBolck);
                detailView.token = view.getToken();
                var entity = new detailView._model();
                var ui = detailView.getControl();
                var signdata = {
                    command: me.meta.command,
                    entityType: me.view.model,
                    parentType: me.view.getParent() ? me.view.getParent().model : ""
                }
                SIE.invokeDataQuery({
                    type: "SIE.Web.Barcodes.Barcodes.DataQuery.BarcodeDataQuery",
                    method: "GetReprintInfo",
                    params: [barcodeId],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        if (res.Success) {
                            var defaultInfo = res.Result;
                            entity.setTimes(defaultInfo.data.items[0].data.Times);
                            entity.setReason(defaultInfo.data.items[0].data.Reason);
                            entity.setTemplateId(defaultInfo.data.items[0].data.TemplateId);
                            entity.setTemplateId_Display(defaultInfo.data.items[0].data.TemplateId_Display);
                            detailView.setData(entity);
                            var win = SIE.Window.show({
                                title: "条码补打".t(),
                                width: 480,
                                height: 250,
                                items: ui,
                                id: "ReprintInfo001",
                                callback: function (btn) {
                                    if (btn == "确定".t()) {
                                        var reprintInfo = detailView.getData().data;
                                        if (!reprintInfo.TemplateId || reprintInfo.TemplateId <= 0) {
                                            SIE.Msg.showMessage("打印模板不能为空".L10N());
                                            return false;
                                        }
                                        if (!reprintInfo.Reason) {
                                            SIE.Msg.showMessage("补打原因不能为空".L10N());
                                            return false;
                                        }
                                        var selModels = view.getSelectionModel();
                                        SIE.invokeDataQuery({
                                            type: "SIE.Web.Barcodes.Barcodes.DataQuery.BarcodeDataQuery",
                                            method: "Reprint",
                                            params: [view.getSelectionIds(), reprintInfo],
                                            async: false,
                                            token: view.token,
                                            logInfo: signdata,
                                            callback: function (res) {
                                                if (res.Success) {
                                                    debugger;
                                                    var rstPrint = res.Result;
                                                    if (rstPrint.ErrMsg !== '') {
                                                        SIE.Msg.showError(rstPrint.ErrMsg);
                                                        return false;
                                                    } else {
                                                        win.close();
                                                        //if (rstPrint.Type === ".siedev") {
                                                        //    var param = { content: rstPrint.Url };
                                                        //    CRT.Workbench.showPageDialog({
                                                        //        id: 'OutBoxReprint_rpt',
                                                        //        text: "条码外标签补打".t(),
                                                        //        url: '/Modules/PrintTemplate/DevPrint',
                                                        //        params: param,
                                                        //        method: 'POST'
                                                        //    });
                                                        //}
                                                        //else if (rstPrint.Type === ".btw") {
                                                        //    var param = "SIE.RegeditBartender:\\" + rstPrint.Url;
                                                        //    window.open(param);
                                                        //}

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
                                                                    CRT.Workbench.showPageDialog({ id: 'OutBoxReprint_rpt', text: "条码外标签补打".t(), method: 'POST', url: printUrl, params: param });
                                                            }
                                                        }

                                                        selModels.deselectAll();
                                                        view.reloadData();
                                                    }
                                                }
                                            }
                                        });
                                        return false;
                                    }
                                }
                            });
                        }
                    }
                });
            }
        });
    }
});