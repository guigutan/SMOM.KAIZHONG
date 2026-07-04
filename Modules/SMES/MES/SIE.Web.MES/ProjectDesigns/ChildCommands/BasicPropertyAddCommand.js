SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.BasicPropertyAddCommand", {
    extend: "SIE.cmd.Add",
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var parent = this.view.getParent();
        if (parent != null) {
            var current = parent.getCurrent();
            entity.setProjectDesignId(current.getId());
        }
    }
})