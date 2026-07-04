SIE.defineCommand('SIE.Web.Equipments.EquipModels.Commands.OpenEquipBOMCommand', {
    meta: { text: "设备BOM", group: "edit", hierarchy: "管理", iconCls: "icon-Search icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.Equipments.Boms.EquipBom,SIE.EMS',
            module: 'SIE.EMS.Equipments.Boms.EquipBom,SIE.EMS',
            title: '设备BOM'.L10N(),
            isAggt: true
        });
    }
});