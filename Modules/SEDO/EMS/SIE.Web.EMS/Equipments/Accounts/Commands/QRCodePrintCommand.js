SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.QRCodePrintCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "二维码打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    /*
     * @override 执行打印
     * @returns{}
     */

    execute: function (view, source) {
        var lotId = view.getCurrent().data.Id;
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Equipments.Accounts.ViewModels.QRCodePrintCfgViewModel",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "PRINT_VIEW",
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
                    type: "SIE.Web.EMS.Equipments.Accounts.DataQuery.EquipAccountDataQueryer",
                    method: "GetPrintTemplateInfo",
                    params: [lotId],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        if (res.Success) {
                            var defaultInfo = res.Result;
                            entity.setTimes(defaultInfo.data.items[0].data.Times);
                            entity.setTemplateId(defaultInfo.data.items[0].data.TemplateId);
                            entity.setTemplateId_Display(defaultInfo.data.items[0].data.TemplateId_Display);
                            detailView.setData(entity);
                            var win = SIE.Window.show({
                                title: "二维码打印".t(),
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
                                        var selModels = view.getSelectionModel();
                                        SIE.invokeDataQuery({
                                            type: "SIE.Web.EMS.Equipments.Accounts.DataQuery.EquipAccountDataQueryer",
                                            method: "Print",
                                            params: [view.getSelectionIds(), reprintInfo, false],
                                            async: false,
                                            token: view.token,
                                            callback: function (res) {
                                                if (res.Success) {
                                                    var rstPrint = res.Result;
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
                                                                    CRT.Workbench.showPageDialog({ id: 'LotPrint_rpt', text: "设备台账-二维码打印".t(), method: 'POST', url: printUrl, params: param });
                                                            }
                                                        }

                                                        selModels.deselectAll();
                                                        view.reloadData();
                                                    }
                                                }
                                                else {
                                                    SIE.Msg.showError(res.Message);
                                                }
                                            }
                                        });
                                        return false;
                                    }
                                }
                            });
                        }
                        else {
                            SIE.Msg.showError(res.Message);
                        }
                    }
                });
            }
        });
    },
});