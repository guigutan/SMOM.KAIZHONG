SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.SaleOrderEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
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