SIE.defineCommand("SIE.Web.MES.PrepareProducts.Commands.PrepareProductDetailAddCommand", {
    extend: "SIE.cmd.Add",
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var parent = me.view.getParent().getCurrent();
        entity.setProFamiliyId(parent.getProductFamilyId());
        entity.setPrepareProductId(parent.getId());
    },
});
