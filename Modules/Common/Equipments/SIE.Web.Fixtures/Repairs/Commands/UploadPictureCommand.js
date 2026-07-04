SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.UploadPictureCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.UploadAttachmentCommand',
    meta: { text: "上传", group: "edit", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        if (view.getParent().getCurrent() == null)
            return false;
        if (view.getParent().getCurrent().isNew() == true)
            return false;
        return true;
    }
});