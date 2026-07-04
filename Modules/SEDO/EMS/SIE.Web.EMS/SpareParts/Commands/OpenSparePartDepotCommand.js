SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.OpenSparePartDepotCommand', {
    meta: { text: "备件仓库", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.SpareParts.SparePartDepot',
            module: listView.module,
            title: '备件仓库'.L10N(),
            isAggt: true
        });
    }
});