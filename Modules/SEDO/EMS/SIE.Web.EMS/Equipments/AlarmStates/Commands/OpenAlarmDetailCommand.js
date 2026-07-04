SIE.defineCommand('SIE.Web.EMS.Equipments.AlarmStates.Commands.OpenAlarmDetailCommand', {
    meta: { text: "报警明细", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.Equipments.AlarmStates.EquipAlarmRecord',
            module: listView.module,            
            title: '报警明细'.L10N(),
            isAggt: true
        });
    }
});