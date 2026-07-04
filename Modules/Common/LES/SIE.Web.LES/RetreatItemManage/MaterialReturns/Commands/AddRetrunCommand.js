SIE.defineCommand("SIE.Web.LES.RetreatItemManage.MaterialReturns.AddRetrunCommand", {
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    execute: function (entity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: me.view.model,
            title: "添加-生产退料",
            isDetail: true,
            viewGroup: "AddDetailPageView",
        });
    }
});