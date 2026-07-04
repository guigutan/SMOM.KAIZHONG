SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands.TreeRoutingAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var parent = this.view.getParent().getCurrent();
        var designProductTreeId = parent.getId();
        var productId = parent.getProductId();
        var productCode = parent.getProductId_Display();
        var productName = parent.getProductName();
        entity.setProductId(productId);
        entity.setProductCode(productCode);
        entity.setProductName(productName);
        entity.setDesignProductTreeId(designProductTreeId);
    }
})