SIE.defineCommand('SIE.Web.EMS.AssetDisposals.Commands.PrintAssetDisposalSparePartSnCommand', {
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {

        if (view.hasSelectedEntities()) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (Ext.isEmpty(item.getSn())) {
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
        var me = this;
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.AssetDisposals.AssetDisposalSparePart",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "PrintAssetDisposalSparePartSnViewGroup",
            callback: function (res) {
                var detailView = SIE.AutoUI.createDetailView(res);
                var entity = new detailView._model();
                detailView.setData(entity);
                var win = SIE.Window.show({
                    title: "打印".t(),
                    width: 480,
                    height: 200,
                    items: detailView.getControl(),
                    id: "PrintAssetDisposalSparePartSn",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var reprintInfo = detailView.getData().data;
                            if (!reprintInfo.PrintTemplateId || reprintInfo.PrintTemplateId <= 0) {
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
            type: "SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer",
            method: "SnPrint",
            params: [view.getSelectionIds(), reprintInfo.PrintTemplateId],
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
                                CRT.Workbench.showPageDialog({ id: 'PrintAssetDisposalSparePartSn_rpt', text: "打印".t(), method: 'POST', url: printUrl, params: param });
                        }
                    }
                    view.reloadData();
                }
            }
        });
    }
});