SIE.defineCommand('SIE.Web.Resources.Employees.Commands.EmployeeGroupCommand', {
    meta: { text: "员工组", group: "edit", iconCls: "icon-People icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Resources.Employees.EmployeeGroup',
            title: '员工组'.L10N(),
            module: view.module,
            isAggt: true
        });
    }
});