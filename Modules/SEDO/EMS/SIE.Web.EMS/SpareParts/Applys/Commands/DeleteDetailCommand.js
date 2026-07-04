SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.DeleteDetailCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        var parentResult = this.callParent(arguments);
        if (parentResult == true) {
            if (view.getParent() && view.getParent().getCurrent()) {
                var auditState = view.getParent().getCurrent().getAuditState();
                return auditState == 0 || auditState == 2;//申请单状态=创建 或=已驳回
            }
            return false;
        }
        else
            return false;
    }
});