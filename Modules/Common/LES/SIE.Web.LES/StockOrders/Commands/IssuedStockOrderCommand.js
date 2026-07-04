SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.IssuedStockOrderCommand', {
    meta: { text: "下发", group: "edit", iconCls: "icon-Submit icon-blue", toolbar:"明细同物料、扩展属性、接收方式会合并" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        let sel = view.getSelection();

        return sel.all(function (p) { return p.getStockState() == SIE.LES.StockOrder.StockState.Submitted.value; });
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('是否确认下发?'.t()), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("下发完成!".t());
                    view.reloadData();
                }
            });
        });
    }
});