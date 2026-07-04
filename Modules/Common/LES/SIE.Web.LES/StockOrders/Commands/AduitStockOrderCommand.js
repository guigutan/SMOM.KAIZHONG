SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.AduitStockOrderCommand', {
    meta: { text: "审核确认", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        let sel = view.getSelection();

        return sel.all(function (p) { return p.getStockState() == SIE.LES.StockOrder.StockState.Audit.value; });
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('是否确认审核?'.t()), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("审核完成!".t());
                    view.reloadData();
                }
            });
        });
    }
});