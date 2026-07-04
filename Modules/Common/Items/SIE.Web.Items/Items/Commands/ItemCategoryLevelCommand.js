SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemCategoryLevelCommand', {
    meta: { text: "分类层级", group: "edit", iconCls: "icon-Type icon-blue"},
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            tabId: 'menu_' + 'SIE.Items.ItemCategoryLevel,SIE.Items'.replace(/[.|,]/g, ''),
            entityType: 'SIE.Items.ItemCategoryLevel,SIE.Items',
            title: '分类层级'.L10N(),
            module: listView.module,
            isAggt: true
        });
    }
});