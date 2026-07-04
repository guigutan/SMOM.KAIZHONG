SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.OpenSparePartStoreCommand', {
    meta: { text: "备件入库", group: "edit", iconCls: "icon-Search icon-blue" },
    execute: function (listView, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.EMS.SpareParts.SparePartStore',
            module: listView.module,
            title: '备件入库'.L10N(),
            isAggt: true
        });
    }
});