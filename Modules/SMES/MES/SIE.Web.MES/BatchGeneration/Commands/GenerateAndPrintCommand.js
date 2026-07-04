SIE.defineCommand('SIE.Web.MES.BatchGeneration.Commands.GenerateAndPrintCommand', {
    meta: { text: "批次生成并过站(打印)", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        var entity = view.getData();
        return entity != null && entity.data.GenerateingQty > 0 && entity.data.BatchQty > 0;
    },
    execute: function (view, source) {
        var me = view;
        var entity = view.getData();
        if (!view.validateData(view)) {
            SIE.Msg.showMessage("输入数据不正确，请重新输入".t());
            return;
        }
        if (entity) {
            Ext.MessageBox.show({
                msg: '正在生成并过站'.t(),
                progressText: '...',
                width: 300,
                modal: true,
                wait: {
                    interval: 200
                }
            });
            me.timer = Ext.defer(function () {
                me.timer = null;
                Ext.MessageBox.hide();
            }, 15000);
            view.execute({
                data: entity.data,
                success: function (res) {
                    var rstPrint = res.Result;

                    if (rstPrint.ErrMsg !== '') {
                        SIE.Msg.showError(rstPrint.ErrMsg);
                        return false;
                    } else {                        
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
                                    CRT.Workbench.showPageDialog({ id: 'WipBatchs_rpt', text: "批次生成打印".t(), method: 'POST', url: printUrl, params: param });

                            }
                        }

                        Ext.getCmp("BatchGeneratingViewModel001").close();
                        Ext.MessageBox.hide();
                        entity.ownerView.reloadData();
                    }
                }
            });
        }
    }
});
