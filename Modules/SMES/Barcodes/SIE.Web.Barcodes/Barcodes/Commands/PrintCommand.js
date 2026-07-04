SIE.defineCommand('SIE.Web.Barcodes.PrintCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    /*
    @override 是否可执行
    @param {} view
    @returns {}
    */
    canExecute: function (view) {
        var curModel = view.getCurrent();

        if (curModel != null) {
            var curData = curModel.getData();
            var printQty = curData.PrintQty; //打印数量
            var residualQty = curData.ResidualQty; //剩余数量
            var singleQty = curData.SingleQty; //单张数量
            if (residualQty > 0 && printQty > 0 && singleQty > 0 && singleQty <= printQty)
                return true;
            else
                return false;
        }
        else
            return false;
    },

    /*
     * @override 执行打印
     * @returns{}
     */
    execute: function (view, source) {
        var current = view.getCurrent();
        if (!this.validataPrint(current))
            return;         
        Ext.MessageBox.show({
            msg: '正在执行'.t(),
            progressText: '...',
            width: 300,
            wait: {
                interval: 200
            }
        });
        var me = this;
        var signdata = {
            command: me.meta.command,
            entityType: me.view.model,
            parentType: me.view.getParent() ? me.view.getParent().model : ""
        }
        SIE.invokeDataQuery({
            type: "SIE.Web.Barcodes.Barcodes.DataQuery.BarcodeDataQuery",
            method: "Print",
            params: [current.data],
            async: false,
            token: view.token,
            logInfo: signdata,
            callback: function (res) {
                if (res.Success) {
                    var rstPrint = res.Result;
                    if (rstPrint.ErrMsg !== '') {
                        SIE.Msg.showError(rstPrint.ErrMsg);
                        return;
                    } else {
                        var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({ ReportType: rstPrint.Type, ReportData: { path: rstPrint.Url, content: rstPrint.Url} });
                        var cfg = printCmpt.getExtTarget();
                        if (cfg && cfg.printCallback) {
                            cfg.printCallback(printCmpt);
                        }
                        else {
                            var param = printCmpt.getPrintParams();
                            if (!printCmpt.hasError()) {
                                var printUrl = printCmpt.getPrintUrl();
                                if (!printCmpt.hasError())
                                    CRT.Workbench.showPageDialog({ id: 'LotPrint_rpt', text: "条码打印".t(), method: 'POST', url: printUrl, params: param });
                            }
                        }
                        var win = Ext.getCmp("BarcodePrintViewModel001");
                        if (win)
                            win.close();                       
                        Ext.MessageBox.hide();
                        view.mainView.reloadData();
                    }                    
                }
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
        else if (model.data.SingleQty == null && model.data.SingleQty <= 0) {
            validateFlag = false;
            SIE.Msg.showError("单张数量不正确!".t());
        }
        else if (model.data.PrintedQty == null && model.data.PrintedQty < 0) {
            validateFlag = false;
            SIE.Msg.showError("已打印数量不正确!".t());
        }
        //打印模板名后缀匹配
        //var Template = model.data.TemplateId_Display;
        //var str= Template.substring(Template.indexOf('.'), Template.length)
        //if (str.toLowerCase() != ".siedev") {
        //    SIE.Msg.showMessage("【打印模板】不正确，模板应选择【.siedev】类型!".t());
        //    validateFlag= false;
        //}
        return validateFlag;
    }

});