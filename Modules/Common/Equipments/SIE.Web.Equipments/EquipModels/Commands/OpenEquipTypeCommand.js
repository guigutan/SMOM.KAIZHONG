SIE.defineCommand('SIE.Web.Equipments.EquipModels.Commands.OpenEquipTypeCommand', {
    meta: { text: "设备类型", group: "edit", hierarchy: "管理", iconCls: "icon-Search icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Equipments.EquipTypes.EquipType,SIE.Equipments',
            module: 'SIE.Equipments.EquipTypes.EquipType,SIE.Equipments',
            title: '设备类型维护'.L10N(),
            isAggt: true
        });
    }
});