SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit" },
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current && !current.isDirty())
            return false;

        this.selectedItems = view.getSelection();
        if (this.selectedItems.length === 0)
            return false;

        var result = true;
        this.selectedItems.forEach(function (p) {
            //5 已完成;8 关闭;
            if (p.getRepairState() === 5 || p.getRepairState() === 8) {
                result = false
            }
        });


        if (!result) {
            return false;
        }

        return this.callParent(arguments);
    },
});