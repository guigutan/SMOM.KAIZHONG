SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.SaleOrderDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var orderList = view.getSelection();
        if (orderList != null && orderList.length > 0) {
            for (var i = 0; i < orderList.length; i++) {
                if (orderList[i].data.DetailSum > 0) {
                    return false;
                }
                return true;
            }
        }
        return false;
    }
})