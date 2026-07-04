SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.ShowShiftTypeCommand', {
    meta: { text: "查看班制", group: "edit", iconCls: "icon-FileEye icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Resources.ShiftTypes.ShiftType,SIE.Resources',
            title: '班制'.L10N(),
            module: 'SIE.Resources.ShiftTypes.ShiftType,SIE.Resources',//view.module,
            ignoreQuery: false,
            isAggt: true
        });
    }
});