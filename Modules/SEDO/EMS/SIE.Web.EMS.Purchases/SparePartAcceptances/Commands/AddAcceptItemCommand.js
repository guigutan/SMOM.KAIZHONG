SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.AddAcceptItemCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
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