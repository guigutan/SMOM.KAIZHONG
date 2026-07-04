SIE.defineCommand('SIE.Web.MES.TaskManagement.Specifications.Commands.SpecificationCategoryCommand', {
    meta: { text: "规格分类维护", group: "edit" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            tabId: 'menu_' + 'SIE.MES.TaskManagement.Specifications.SpecificationCategory,SIE.MES.TaskManagement'.replace(/[.|,]/g, ''),
            entityType: 'SIE.MES.TaskManagement.Specifications.SpecificationCategory,SIE.MES.TaskManagement',
            title: '规格分类维护'.L10N(),
            module: listView.module,
            isAggt: true
        });
    }
});