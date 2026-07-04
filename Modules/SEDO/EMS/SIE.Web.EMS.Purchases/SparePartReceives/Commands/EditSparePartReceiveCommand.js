SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.EditSparePartReceiveCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.ReceiveBillStatus !== 10) return false;

        //有接收则按钮不可用
        if (p.data.HasReceived) {
            return false;
        }

        return true;
    }
});