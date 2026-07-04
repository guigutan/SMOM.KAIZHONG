SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.CloseLesStockCountCommand', {
    meta: { text: "关闭", group: "edit", iconCls: "icon-Cancel icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0 || view.getSelection().length > 1) {
            return false;
        }
        var sel = view.getSelection();
        return sel.all(function (p) {
            //SIE.Warehouses.CountState.Audit.value(审批)=10
            //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
            //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
            return p.getState() === 10 || p.getState() === 30 || p.getState() === 40;
        });
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion('关闭单据后将不能进行编辑，是否继续？'.t(), function () {
            SIE.Msg.wait("正在关闭......".t());
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showMessage('关闭成功!'.t());
                    view.reloadData();
                }
            });
        });

    }
});