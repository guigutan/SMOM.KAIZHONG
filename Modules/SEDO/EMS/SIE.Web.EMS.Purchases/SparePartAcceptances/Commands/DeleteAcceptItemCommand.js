SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.DeleteAcceptItemCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var parent = view._parent.getCurrent();
        if (parent == null || parent.data == null) {
            return false;
        }
        if (parent.data.ApprovalStatus !== 10 && parent.data.ApprovalStatus !== 50) {
            return false;
        }
        if (view.hasSelectedEntities()) {
            return true;
        }
        return false;
    }
});