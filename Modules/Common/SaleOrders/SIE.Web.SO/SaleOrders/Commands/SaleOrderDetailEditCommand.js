SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.SaleOrderDetailEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.data) {
            if (entity.data.LineState != SIE.Pcb.SO.SaleOrders.LineState.NEW.value) {
                return false;
            }
            return true;
        }
        return false;
    }
})