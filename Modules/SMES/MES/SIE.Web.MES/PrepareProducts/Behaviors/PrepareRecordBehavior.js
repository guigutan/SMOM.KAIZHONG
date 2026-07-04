Ext.define("SIE.Web.MES.PrepareProducts.Behaviors.PrepareRecordBehavior", {
    onDataLoaded: function (view) {
        var me = this;
        me.view = view;

        var entity = CRT.Context.PageContext.getCurrentRecord();
        var params = CRT.Context.PageContext.getParams();
        var workOrderId = params.WorkOrderId;
        SIE.invokeDataQuery({
            method: "CreatePrepareRecordDetail",
            params: [workOrderId],
            async: true,
            action: 'queryer',
            type: "SIE.Web.MES.PrepareProducts.DataQuerys.PrepareRecordDataQuery",
            token: view.token,
            success: function (res) {
                var data = res.Result;
                var view = me.view;
                view._children[0].getData().removeAll();
                view._children[0].getControl().setStore(res.Result.data.items);
                debugger;
                debugger;
            }
        });
    },
});
