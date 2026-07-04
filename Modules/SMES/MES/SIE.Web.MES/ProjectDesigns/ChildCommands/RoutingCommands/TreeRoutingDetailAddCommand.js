SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands.TreeRoutingDetailAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var lineNo = 10;
        var list = this.view.getData().data.items;
        if (list.length > 0) {
            lineNo = list.select(function (p) { return p.getIndex() }).max() + 10;
        }
        else {
            lineNo = 10;
        }
        entity.setIndex(lineNo);
    }
})