SIE.defineCommand("SIE.Web.MES.PrepareProducts.Commands.PrepareRecordExecuteCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "执行", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getCurrent() === null)
        {
            return false;
        }
        if (view.getSelection().length != 1)
        {
            return false;
        }
        if (view.getCurrent() && view.getCurrent().getData().PrepareState != 0)
        {
            return false;
        }
        //var workOrderId = view.getCurrent().getData().Id;
        //SIE.invokeDataQuery({
        //    method: "CreatePrepareRecordDetail",
        //    params: [workOrderId],
        //    async: false,
        //    action: 'queryer',
        //    type: "SIE.Web.MES.PrepareProducts.DataQuerys.PrepareRecordDataQuery",
        //    token: view.token,
        //    success: function (res) {
        //        if (res.Result.data.items.length > 0) {
        //            return true;
        //        } else {
        //            return false;
        //        }
        //    }
        //}); 有性能影响

        return true;
    },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            CRT.Workbench.addPage({
                entityType: me.view.model,
                title: "执行".t(),
                recordId: entity.getId(),
                viewGroup: "ExecuteViewStr",
                params: {
                    WorkOrderId: entity.getId(),
                },
                isDetail: true,
            });
        }
    },
});
