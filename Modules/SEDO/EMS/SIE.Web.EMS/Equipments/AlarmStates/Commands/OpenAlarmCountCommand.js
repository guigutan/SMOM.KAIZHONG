SIE.defineCommand('SIE.Web.EMS.Equipments.AlarmStates.Commands.OpenAlarmCountCommand', {
    meta: { text: "报警统计", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.Equipments.AlarmStates.AlarmCount',
            module: listView.module,
            title: '报警统计'.L10N(),
            isAggt: true
        });
    }
});