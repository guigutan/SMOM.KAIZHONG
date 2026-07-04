SIE.defineCommand('SIE.Web.Items.ProductFamilys.Commands.ProductFamilyCommand', {
    meta: { text: "族分类维护", group: "edit", iconCls: "icon-FormatListBulleted icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.Items.ProductFamilyCategory',
            module: listView.module,
            title: '产品族分类维护'.L10N(),
            isAggt: true
        });
    }
});
