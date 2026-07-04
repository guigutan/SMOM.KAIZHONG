SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.ProductTreeSaveCommand", {
    extend: "SIE.cmd.Save",
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaved: function (view, res) {
        var me = this;
        view.reloadData();
        me.onSavedMsg(view, res);
        me.refreshOtherView(view);
    },
    // 刷新另外两个视图数据
    refreshOtherView: function (view) {
        // 产品Bom
        var bomView = view.getParent().findChild("SIE.MES.ProjectDesigns.ChildInfos.DesignTreeBom");
        bomView.reloadData();
        // 产品工艺路线设计
        var routingView = view.getParent().findChild("SIE.MES.ProjectDesigns.ChildInfos.DesignTreeRouting");
        routingView.reloadData();
    }
})