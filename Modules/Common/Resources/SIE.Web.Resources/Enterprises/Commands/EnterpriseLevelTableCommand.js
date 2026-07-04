SIE.defineCommand('SIE.Web.Resources.Commands.EnterpriseLevelTableCommand', {
    meta: { text: "企业层级", group: "edit", iconCls: "icon-FormatListBulleted icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Resources.Enterprises.EnterpriseLevel',
            title: '企业层级'.L10N(),
            module: listView.module,
            isAggt: true
        });
    }
});