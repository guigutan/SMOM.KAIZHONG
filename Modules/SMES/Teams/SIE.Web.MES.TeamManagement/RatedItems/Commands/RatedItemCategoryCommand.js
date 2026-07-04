SIE.defineCommand('SIE.Web.MES.TeamManagement.RatedItems.Commands.RatedItemCategoryCommand', {
    meta: { text: "分类维护", group: "edit", iconCls: "icon-Repair icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.MES.TeamManagement.RatedItems.RatedItemCategory,SIE.MES.TeamManagement',
            title: '项目分类'.L10N(),
            module: view.module,
            isAggt: true
        });
    }
});