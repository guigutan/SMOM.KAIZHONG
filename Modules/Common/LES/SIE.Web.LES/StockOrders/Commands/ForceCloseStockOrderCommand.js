SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.ForceCloseStockOrderCommand', {
    meta: { text: "强制关闭", group: "edit", iconCls: "icon-Cancel icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        let sel = view.getSelection();
        //备料单状态已提交、已下发、已拣配、拣配中、待接收、已接收允许点击强制关闭按钮
        return sel.all(function (p) {
            return p.getStockState() === 20
                || p.getStockState() === 70
                || p.getStockState() === 25
                || p.getStockState() === 40
                || p.getStockState() === 50
        });
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('是否确认强制关闭?'.t()), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage("强制关闭完成!".t());
                    view.reloadData();
                }
            });
        });
    }
});