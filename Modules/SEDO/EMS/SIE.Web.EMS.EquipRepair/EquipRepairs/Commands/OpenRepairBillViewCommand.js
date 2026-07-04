SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.OpenRepairBillViewCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "打开", group: "edit", iconCls: "icon-Search icon-blue" },
    canExecute: function (listView) {
        var current = listView.getCurrent();
        if (current === null) { return false; }

        return true;
    },
    execute: function (listView, source) {
        var rawId = "SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill,SIE.EMS.EquipRepair";
        var no = listView.getCurrent().getRepairNo();
        var tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
        CRT.Workbench.addPage({
            tabId: tabId,
            entityType: 'SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill',
            module: listView.module,
            title: ('设备维修管理_' + no).L10N(),
            isAggt: true,
            params: {
                RepairNo: listView.getCurrent().getRepairNo()
            }
        });
    }
});