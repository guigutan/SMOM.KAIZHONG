SIE.defineCommand('SIE.Web.MES.PackingPrints.Commands.PrintCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        return view.getCurrent() && view.getCurrent().getPrintQty() > 0;
    },
    execute: function (view, source) {
        var me = this;
        var current = view.getCurrent();
        if (!this.validataPrint(current))
            return;
        var isPrint = false;
        if (current.data.PrintQty > current.data.ResidualQty)
            isPrint = true;
        if (isPrint) {
            SIE.Msg.askQuestion('打印数已经超出剩余数，是否继续打印？'.t(), function () {
                me.performPrint(view, current);
            });
        }
        else {
            me.performPrint(view, current);
        }
    },
    performPrint: function (view,current) {
        Ext.getCmp("PackingBarcodePrintViewModel001").close();
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
                var param = { content: res.Result };
                CRT.Workbench.showPageDialog({
                    id: 'PackingPrint_rpt',
                    text: "包装号打印".t(),
                    url: '/Modules/PrintTemplate/DevPrint',
                    params: param,
                    method: 'POST'
                });
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
        else if (model.data.PackageRuleDetailId == null || model.data.PackageRuleDetailId <= 0) {
            validateFlag = false;
            SIE.Msg.showError("包装规则未设置!".t());
        }
        else if (model.data.TemplateId == null || model.data.TemplateId <= 0) {
            validateFlag = false;
            SIE.Msg.showError("打印模板未设置!".t());
        }
        else if (model.data.PrintQty == null || model.data.PrintQty <= 0) {
            validateFlag = false;
            SIE.Msg.showError("打印数量不正确!".t());
        }
        return validateFlag;
    }
});