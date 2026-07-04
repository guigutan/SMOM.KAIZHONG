SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.DocumentCommands.DocumentAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var parent = me.view.getParent().getCurrent();
        entity.setProjectDesignId(parent.getId());
    }
})