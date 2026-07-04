SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.EditAcceptItemCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) {
            return false;
        }
        var parent = view._parent.getCurrent();
        if (parent == null || parent.data == null) {
            return false;
        }
        if (parent.data.ApprovalStatus !== 10 && parent.data.ApprovalStatus !== 50) {
            return false;
        }
        return true;
    }
});