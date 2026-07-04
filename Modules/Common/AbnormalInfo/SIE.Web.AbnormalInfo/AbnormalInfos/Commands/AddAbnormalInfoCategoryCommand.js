SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalInfos.Commands.AddAbnormalInfoCategoryCommand', {
    meta: { text: "异常分类", group: "edit", iconCls: "icon-AddEntity icon-green" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.AbnormalInfo.AbnormalInfos.AbnormalInfoCategory',
            title: '异常信息分类'.L10N(),
            module: view.module,
            ignoreQuery: false,
            isAggt: true
        });
    }
});