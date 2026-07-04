SIE.defineCommand('SIE.Web.Barcodes.Panels.Commands.PrintCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {
        return view.getCurrent() && view.getCurrent().getPrintQty() > 0;
    },
    execute: function (view, source) {
        var current = view.getCurrent();
        if (!this.validataPrint(current))
            return;
        Ext.getCmp("PanelPrintViewModel001").close();
        Ext.MessageBox.show({
            msg: '正在执行'.t(),
            progressText: '...',
            width: 300,
            wait: {
                interval: 200
            }
        });
        view.execute({
            data: current.data,
            success: function (res) {
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
                            CRT.Workbench.showPageDialog({ id: 'PanelPrint_rpt', text: "拼板码打印".t(), method: 'POST', url: printUrl, params: param });
                    }
                }
                Ext.MessageBox.hide();
                view.mainView.reloadData();
            }
        });
    },

    /*
    * 打印前的检查
    */
    validataPrint: function (model) {
        var validateFlag = true;
        if (model.data.WorkOrderId == null || model.data.WorkOrderId <= 0) {
            validateFlag = false;
            SIE.Msg.showError("工单信息不正确!".t());
        }
        else if (model.data.NumberRuleId == null || model.data.NumberRuleId <= 0) {
            validateFlag = false;
            SIE.Msg.showError("条码规则未设置!".t());
        }
        else if (model.data.TemplateId == null || model.data.TemplateId <= 0) {
            validateFlag = false;
            SIE.Msg.showError("标签模板未设置!".t());
        }
        else if (model.data.PrintQty == null || model.data.PrintQty <= 0) {
            validateFlag = false;
            SIE.Msg.showError("打印数量不正确!".t());
        }
        return validateFlag;
    }
});