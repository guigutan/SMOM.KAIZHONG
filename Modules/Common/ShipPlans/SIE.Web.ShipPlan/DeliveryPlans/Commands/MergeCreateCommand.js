SIE.defineCommand('SIE.Web.ShipPlan.Commands.MergeCreateCommand', {
    meta: { text: "合并创单规则", group: "edit", iconCls: "icon-Merge icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            tabId: 'menu_' + 'SIE.ShipPlan.MergeCreateRule,SIE.ShipPlan'.replace(/[.|,]/g, ''),
            entityType: 'SIE.ShipPlan.MergeCreateRule,SIE.ShipPlan',
            title: '合并创单规则'.L10N(),
            module: listView.module,
            isAggt: true
        });
    }
});