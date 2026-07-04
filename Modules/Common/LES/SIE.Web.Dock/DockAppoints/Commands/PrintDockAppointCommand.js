SIE.defineCommand('SIE.Web.Dock.DockAppoints.Commands.PrintDockAppointCommand', {
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var billIds = view.getSelectionIds();
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.Dock.DockAppoints.DataQueryer.DockAppointDataQueryer",
            method: 'GetDockAppointDefaultTemplate',
            token: view.token,
            params: [],
            success: function (res) {
                me.showWin(view, res.Result, billIds);
            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
            }
        });
    },
    showWin: function (view, entityData, billIds) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: 'SIE.Dock.ViewModels.DockAppointPrintViewModel',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                if (entityData && entityData != null && entityData.TemplateId > 0) {
                    entity.setPrintTemplateId(entityData.TemplateId);
                    entity.setPrintTemplateId_Display(entityData.TemplateFileName);
                }
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "打印".t(),
                    width: 400,
                    height: 240,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var indata = detailView.getCurrent().data;
                            if (indata.PrintTemplateId == null) {
                                SIE.Msg.showError("模板不能为空!".t());
                                return false;
                            }
                            else {
                                view.execute({
                                    data: { BillIds: billIds, PrintTemplateId: indata.PrintTemplateId },
                                    success: function (r) { //回调
                                        me.ShowPrintPreview(r);
                                        win.close();
                                    }
                                });
                            }
                        }
                    }
                });
            }
        });
    },
    /**
     * 弹出打印预览
     * @param {any} view 选择模板的视图
     * @param {any} rst 打印命令返回的数据
    */
    ShowPrintPreview: function (res) {
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
                    CRT.Workbench.showPageDialog({ id: 'Label_rpt', text: "打印".t(), method: 'POST', url: printUrl, params: param });
            }
        }
    },
});