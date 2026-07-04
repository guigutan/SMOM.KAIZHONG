SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.StockOrderMergeIssuedDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        let sel = view.getSelection();
        return sel.all(function (p) { return p.getState() == 0 });
    },    
})