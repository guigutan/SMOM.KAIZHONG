SIE.defineCommand("SIE.Web.MES.ProjectDesigns.Behaviors.ProjectDesignDetailBehavior", {
    onViewReady: function (view) {
        var me = this;
        view.getControl().up().setMinHeight(30); // 拉高子视图
    }
})