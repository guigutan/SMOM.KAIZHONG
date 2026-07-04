SIE.defineCommand('SIE.Web.Defects.Commands.AddDefectResponsibilityCategoryCommand', {
    meta: { text: "分类维护", group: "edit", iconCls: "icon-FormatListBulleted icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Defects.DefectResponsibilityCategory',
            title: '缺陷责任分类'.L10N(),
            module: view.module,
            ignoreQuery: false,
            isAggt: true
        });
    }
});