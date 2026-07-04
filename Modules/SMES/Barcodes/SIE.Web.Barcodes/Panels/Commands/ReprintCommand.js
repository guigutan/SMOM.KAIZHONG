SIE.defineCommand('SIE.Web.Barcodes.Panels.Commands.ReprintCommand', {
    meta: { text: "补打", group: "edit", iconCls: "icon-PrintData icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        var first = selectModels[0].data.WorkOrderId;
        SIE.each(selectModels, function (model) {
            if (model.data.WorkOrderId != first) {
                res = false;
                return;
            }
            if (model.data.IsScrap) {
                res = false;
                return;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var barcodeId = view.getCurrent().data.Id;
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
                SIE.invokeDataQuery({
                    type: "SIE.Web.Barcodes.Panels.DataQueryers.PanelPrintDataQuery",
                    method: "GetReprintInfo",
                    params: [],
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
                                title: "拼板码补打".t(),
                                width: 480,
                                height: 250,
                                items: ui,
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
                                        view.execute({
                                            data: { BarCodeIds: view.getSelectionIds(), Reason: reprintInfo.Reason, Times: reprintInfo.Times, TemplateId: reprintInfo.TemplateId },
                                            withIds: true,
                                            selectIds: view.getSelectionIds(),
                                            success: function (res) {
                                                win.close();
                                                var rstPrint = res.Result;
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
                                                            CRT.Workbench.showPageDialog({ id: 'PanelReprint_rpt', text: "拼板码补打".t(), method: 'POST', url: printUrl, params: param });
                                                    }
                                                }
                                                selModels.deselectAll();
                                                view.reloadData();
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