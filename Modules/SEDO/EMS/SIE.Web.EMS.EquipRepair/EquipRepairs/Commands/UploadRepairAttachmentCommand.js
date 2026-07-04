SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.UploadRepairAttachmentCommand', {
    extend: 'SIE.Web.Core.Common.Commands.UploadZipAttachmentCommand',
    meta: { text: "上传", group: "edit", iconCls: "icon-Upload icon-blue" },
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();
            var item = parentEntity;
            if (item.data.RepairState == 5 || item.data.RepairState == 7 || item.data.RepairState == 8)
                return false;
            return true;
        }
        else
            return false;
    },
});