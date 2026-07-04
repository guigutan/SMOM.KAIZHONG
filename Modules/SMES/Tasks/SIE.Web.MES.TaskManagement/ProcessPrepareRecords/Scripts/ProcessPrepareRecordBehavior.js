Ext.define("SIE.Web.MES.ProcessPrepareRecords.Scripts.ProcessPrepareRecordBehavior", {
    onDataLoaded: function (view) {
        var me = this;
        me.view = view;

        var entity = CRT.Context.PageContext.getCurrentRecord();
        var params = CRT.Context.PageContext.getParams();
        var workOrderId = params.WorkOrderId;
        var id = params.Id;
        SIE.invokeDataQuery({
            method: "CreateProcessPrepareRecordDetail",
            params: [id],
            async: true,
            action: 'queryer',
            type: "SIE.Web.MES.TaskManagement.ProcessPrepareRecords.ProcessPrepareRecordsQuery",
            token: view.token,
            success: function (res) {
                var data = res.Result;
                var view = me.view;
                view._children[0].getData().removeAll();
                view._children[0].getControl().setStore(res.Result.data.items);
            }
        });
    },
});
