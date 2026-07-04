SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.DeleteAcceptanceAttachmentCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.DeleteAttachmentCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var cur = view.getParent().getCurrent();
        if (cur == null)
            return false;
        if (cur.data.ApprovalStatus !== 10 && cur.data.ApprovalStatus !== 50)
            return false;
        if (view.hasSelectedEntities())
            return true;
        return false;
    }
});