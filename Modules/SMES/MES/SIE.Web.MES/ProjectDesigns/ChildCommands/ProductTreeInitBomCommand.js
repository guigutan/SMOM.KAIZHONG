SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.ProductTreeInitBomCommand", {
    meta: { text: "引用标准Bom", group: "edit", iconCls: "icon-SectionExpandAll icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view) {
        var me = this;
        var parent = view.getParent().getData();
        var store = view.getData();
        if (store != null && store.data.items.length > 0) {
            
            SIE.Msg.askQuestion("引用标准Bom会覆盖当前数据，是否继续？".t(), function () {
                me.initBom(view, parent);
            });
        }
        else {
            me.initBom(view, parent);
        }
    },
    initBom: function (view, parent) {
        var me = this;
        var data = {
            ProductId: parent.getProductId(),
            ProjectDesignDetailId: parent.getId(),
        }

        SIE.Msg.wait("正在引用标准Bom...".t());
        view.execute({
            data: data,
            success: function (res) {
                if (res.Success) {
                    view.reloadData();
                    me.refreshOtherView(view);
                    SIE.Msg.close();
                }
            }
        })
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