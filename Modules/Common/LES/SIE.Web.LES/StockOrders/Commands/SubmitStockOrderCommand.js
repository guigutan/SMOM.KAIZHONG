SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.SubmitStockOrderCommand', {
    meta: { text: "提交审核", group: "edit", iconCls: "icon-Submit icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        let sel = view.getSelection();

        return sel.all(function (p) { return p.getStockState() == SIE.LES.StockOrder.StockState.Created.value; });
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('是否确认提交?'.t()), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("提交完成!".t());
                    view.reloadData();
                }
            });
        });
    }
});