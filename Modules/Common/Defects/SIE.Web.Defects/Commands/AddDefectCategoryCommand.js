SIE.defineCommand('SIE.Web.Defects.Commands.AddDefectCategoryCommand', {
    meta: { text: "分类维护", group: "edit", iconCls: "icon-FormatListBulleted icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Defects.DefectCategory',
            title: '缺陷代码分类'.L10N(),
            module: view.module,
            isAggt: true
        });
    }
});