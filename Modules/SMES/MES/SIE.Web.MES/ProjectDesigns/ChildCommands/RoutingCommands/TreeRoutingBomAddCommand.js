SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands.TreeRoutingBomAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        var cur = view.getParent().getCurrent();
        if (cur == null) {
            return false;
        }
        if (!cur.getHasRoutingDetail()) {
            return false;
        }
        return true;
    },
    onItemCreated: function (entity) {
        var me = this;
        var parent = this.view.getParent().getCurrent();
        var productId = parent.getProductId();
        entity.setProductId(productId);
    }
})