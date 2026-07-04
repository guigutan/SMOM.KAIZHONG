SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectCloseCommand', {
    meta: { text: "关闭", group: "edit", hierarchy: "项目操作", iconCls: "icon-ClipboardPaperCheck icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.EarlierStage.Projects.ProjectClose',
            title: '项目结项'.t(),
            module: 'SIE.EMS.EarlierStage.Projects.ProjectClose,SIE.EMS.EarlierStage',
            isAggt: true
        });
    }
});