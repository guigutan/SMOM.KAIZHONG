SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.UploadPurReqAttachmentCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.UploadAttachmentCommand',
    meta: { text: "上传", group: "edit", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        var cur = view.getParent().getCurrent();
        if (cur == null)
            return false;
        if (cur.data.ApprovalStatus !== 10 && cur.data.ApprovalStatus !== 50)
            return false;
        return true;
    }
});