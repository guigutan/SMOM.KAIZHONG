SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.DeleteEquipRepairSparePartChgCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    isImmediate: true,
    selectedItems: [],
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();

            this.selectedItems = listview.getSelection();
            if (this.selectedItems.length === 0)
                return false;
            if (this.selectedItems.any(function (p) { return p.data.State == 1; }))
                return false;

            var curId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId.toString();
            var item = parentEntity;
            var employeeIdsArr = [Ext.isEmpty(item.data.RepairMasterId) ? "" : item.data.RepairMasterId.toString()];
            if (!Ext.isEmpty(item.data.RepairEmployeeIds))
                employeeIdsArr = employeeIdsArr.concat(item.data.RepairEmployeeIds.split(','));
            if (item.data.RepairState == 0 || item.data.RepairState == 4 || item.data.RepairState == 5 || item.data.RepairState == 7 || item.data.RepairState == 8 || employeeIdsArr.indexOf(curId) < 0)
                return false;
            return true;
        }
        else
            return false;
    },
})