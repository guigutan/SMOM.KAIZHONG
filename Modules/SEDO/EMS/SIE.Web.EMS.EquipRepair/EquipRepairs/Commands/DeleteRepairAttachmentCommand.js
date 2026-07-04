SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.DeleteRepairAttachmentCommand', {
    extend: 'SIE.Web.Common.Attachments.Commands.DeleteAttachmentCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-Delete icon-red" },
    selectedItems: [],
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();

            this.selectedItems = listview.getSelection();
            if (this.selectedItems.length === 0)
                return false;

            var item = parentEntity;
            if (item.data.RepairState == 5 || item.data.RepairState == 7 || item.data.RepairState == 8)
                return false;
            return true;
        }
        else
            return false;

    },
});