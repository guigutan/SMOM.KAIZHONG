SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.ShowHolidayCommand', {
    meta: { text: "查看法定假期", group: "edit", iconCls: "icon-FileEye icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Resources.Holidays.Holiday,SIE.Resources',
            title: '法定假期'.L10N(),
            module: 'SIE.Resources.Holidays.Holiday,SIE.Resources',//view.module,
            ignoreQuery: false,
            isAggt: true
        });
    }
});