SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.CopyAcceptItemCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-ContentCopy icon-green", splitTo: "添加" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
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