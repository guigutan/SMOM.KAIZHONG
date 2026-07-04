SIE.defineCommand('SIE.Web.Kit.TurnoverTools.Commands.TurnoverToolModelTableCommand', {
    meta: { text: "周转工具型号维护", group: "edit" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.TurnoverTools.TurnoverTools.TurnoverToolModel',
            title: '周转工具型号维护'.L10N(),
            module: listView.module,
            isAggt: true
        });
    }
});