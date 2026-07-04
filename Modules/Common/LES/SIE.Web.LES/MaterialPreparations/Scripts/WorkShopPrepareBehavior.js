Ext.define("SIE.Web.LES.MaterialPreparations.Scripts.WorkShopPrepareBehavior", {

    onViewReady: function (view) {
        var me = this;
        this.view = view;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            entity.setNo(params.No);
            entity.setPrepareType(params.PrepareType);
        }
    }
});