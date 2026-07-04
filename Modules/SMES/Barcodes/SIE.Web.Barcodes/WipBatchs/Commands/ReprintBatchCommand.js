SIE.defineCommand('SIE.Web.Barcodes.WipBatchs.Commands.ReprintBatchCommand', {
    meta: { text: "补打", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        return true;
    },
    execute: function (view, source) {
        var wipBatchId = view.getCurrent().data.Id;
        SIE.AutoUI.getMeta({
            model: "SIE.Web.Barcodes.WipBatchs.ViewModels.BatchReprintViewModel",
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
                entity.setIsSubBatch(false);
                var ui = detailView.getControl();
                SIE.invokeDataQuery({
                    type: "SIE.Web.Barcodes.WipBatchs.WipBatchsDataQueryer",
                    method: "GetReprintInfo",
                    params: [wipBatchId],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        if (res.Success) {
                            var defaultInfo = res.Result;
                            entity.setTemplateId(defaultInfo.data.items[0].data.TemplateId);
                            entity.setTemplateId_Display(defaultInfo.data.items[0].data.TemplateId_Display);
                            detailView.setData(entity);
                            var win = SIE.Window.show({
                                title: "生产批次补打".t(),
                                width: 480,
                                height: 250,
                                items: ui,
                                id: "ReprintBatch001",
                                callback: function (btn) {
                                    if (btn == "确定".t()) {
                                        var reprintInfo = detailView.getData().data;
                                        if (!reprintInfo.TemplateId || reprintInfo.TemplateId <= 0) {
                                            SIE.Msg.showMessage("打印模板不能为空".L10N());
                                            return false;
                                        }
                                        view.execute({
                                            data: { WipBatchIds: view.getSelectionIds(), TemplateId: reprintInfo.TemplateId },
                                            success: function (res) {
                                                win.close();

                                                var rstPrint = res.Result;

                                                if (rstPrint.ErrMsg !== '') {
                                                    SIE.Msg.showError(rstPrint.ErrMsg);
                                                    return false;
                                                } else {
                                                    win.close();

                                                    var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({
                                                        ReportType: rstPrint.Type,
                                                        ReportData: {
                                                            path: rstPrint.Url, content: rstPrint.Url
                                                        }
                                                    });

                                                    var cfg = printCmpt.getExtTarget();

                                                    if (cfg && cfg.printCallback) {
                                                        cfg.printCallback(printCmpt);
                                                    }
                                                    else {
                                                        var param = printCmpt.getPrintParams();
                                                        if (!printCmpt.hasError()) {
                                                            var printUrl = printCmpt.getPrintUrl();
                                                            if (!printCmpt.hasError())
                                                                CRT.Workbench.showPageDialog({ id: 'ReprintBatch_rpt', text: "生产批次补打".t(), method: 'POST', url: printUrl, params: param });
                                                        }
                                                    }

                                                    view.getSelectionModel().deselectAll();

                                                    view._parent.reloadData();
                                                }
                                            },
                                            error: function (res) {
                                                SIE.Msg.showMessage(res.Message);
                                                return false;
                                            }
                                        });
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