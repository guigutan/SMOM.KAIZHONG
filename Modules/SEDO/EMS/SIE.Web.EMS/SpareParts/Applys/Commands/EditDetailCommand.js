SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.EditDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-Edit icon-blue" },
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
    },
});