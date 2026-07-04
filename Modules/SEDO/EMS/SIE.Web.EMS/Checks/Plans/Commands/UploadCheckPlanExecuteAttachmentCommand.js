SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.UploadCheckPlanExecuteAttachmentCommand', {
    extend: 'SIE.Web.Core.Common.Commands.UploadZipAttachmentCommand',
    meta: { text: "上传", group: "edit", iconCls: "icon-Upload icon-blue" },
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();
            var item = parentEntity;
            if (item.data.ExeState == 1) {
                return false;
            }
            return true;
        }
        else {
            return false;
        }
    },
});