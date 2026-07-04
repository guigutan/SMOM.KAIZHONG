SIE.defineCommand('SIE.Web.Equipments.EquipModels.Commands.OpenEquipAccountCommand', {
    meta: { text: "设备台账", group: "edit", hierarchy: "管理", iconCls: "icon-Search icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Equipments.EquipAccounts.EquipAccount,SIE.Equipments',
            module: 'SIE.Equipments.EquipAccounts.EquipAccount,SIE.Equipments',
            title: '设备台账维护'.L10N(),
            isAggt: true
        });
    }
});