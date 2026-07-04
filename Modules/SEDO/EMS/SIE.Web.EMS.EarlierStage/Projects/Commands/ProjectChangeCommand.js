SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectChangeCommand', {
    meta: { text: "变更", group: "edit", hierarchy: "项目操作", iconCls: "icon-FormatListBulletedType icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.EarlierStage.Projects.ProjectChange',
            title: '项目变更'.t(),
            module: 'SIE.EMS.EarlierStage.Projects.ProjectChange,SIE.EMS.EarlierStage',
            isAggt: true
        });
    }
});