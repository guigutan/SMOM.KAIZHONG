SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.ReCallStockOrderCommand', {
    meta: { text: "撤销", group: "edit", iconCls: "icon-Reload icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        let sel = view.getSelection();

        return sel.all(function (p) { return p.getStockState() == SIE.LES.StockOrder.StockState.Created.value || p.getStockState() == SIE.LES.StockOrder.StockState.Audit.value; });
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('是否确认撤回?'.t()), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("撤回完成!".t());
                    view.reloadData();
                }
            });
        });
    }
});