SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.SaleOrderDetailDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 0) {
            for (var i = 0; i < selecteditems.length; i++) {
                if (selecteditems[i].data.LineState != SIE.Pcb.SO.SaleOrders.LineState.NEW.value) {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
})