SIE.defineCommand("SIE.Web.LES.MaterialReceptions.Commands.MaterialReceptionAddCommand", {
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    execute: function (entity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: me.view.model,
            title: "扫描页面".t(),
            isDetail: true
        });
    }
});

