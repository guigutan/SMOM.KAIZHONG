SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.BomCommands.TreeBomAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var parent = me.view.getParent().getCurrent();
        var designProductTreeId = parent.getId();
        var productId = parent.getProductId();
        var productCode = parent.getProductId_Display();
        var productName = parent.getProductName();
        this.view.execute({
            data: model,
            isSubmmit: false,
            success: function (res) {
                var version = res.Result;
                entity.setVersion(version);
                entity.setProductId(productId);
                entity.setProductCode(productCode);
                entity.setProductName(productName);
                entity.setDesignProductTreeId(designProductTreeId);
            }
        }, me.view);
    }
})