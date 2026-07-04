SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.StockOrderMergeIssuedEnableCommand', {
    meta: { text: "启用", group: "business", iconCls: "icon-NetworkNormal icon-green" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        let sel = view.getSelection();
        return sel.any(function (p) { return p.getState() == 0 });
    },
    execute: function (view) {
        var current = view.getCurrent();
        this.view.execute({
            data: view.getSelectionIds(),
            isSubmmit: false,
            success: function (res) {
                view.reloadData();
            }
        }, view);
    }
})