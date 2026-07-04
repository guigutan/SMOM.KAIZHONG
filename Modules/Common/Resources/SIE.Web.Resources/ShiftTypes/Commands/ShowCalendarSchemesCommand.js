SIE.defineCommand('SIE.Web.Resources.ShiftTypes.Commands.ShowCalendarSchemesCommand', {
    meta: { text: "查看日历方案", group: "edit", iconCls: "icon-FileEye icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Resources.CalendarSchemes.CalendarScheme,SIE.Resources',
            title: '日历方案'.L10N(),
            module: 'SIE.Resources.CalendarSchemes.CalendarScheme,SIE.Resources',//view.module,
            ignoreQuery: false,
            isAggt: true
        });
    }
});