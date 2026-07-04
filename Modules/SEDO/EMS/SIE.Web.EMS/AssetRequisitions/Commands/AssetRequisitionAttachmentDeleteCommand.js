SIE.defineCommand('SIE.Web.EMS.AssetRequisitions.Commands.AssetRequisitionAttachmentDeleteCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.DeleteAttachmentCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-Delete icon-blue" },
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();
            var item = parentEntity;
            if (item.data.ApprovalStatus == 40) {
                return false;
            }
            return true;
        }
        else {
            return false;
        }
    },
});
