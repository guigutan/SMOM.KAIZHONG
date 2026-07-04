SIE.defineCommand("SIE.Web.MES.BarcodeProcesses.Commands.SynWoProcessListCommand", {
    meta: { text: "同步工序清单", group: "edit", iconCls: "icon-Redo icon-blue" },
    canExecute: function (view) {
        var parent = view._parent;
        if (parent == null) {
            return false;
        }
        var parentData = parent.getCurrent();
        if (parentData == null) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var parentData = view._parent.getCurrent();
        var mainId = parentData.getId();
        var woId = parentData.getWorkOrderId();
        SIE.Msg.wait("正在同步工单工序清单......".t());
        SIE.invokeDataQuery({
            method: 'SynWoProcessListQuery',
            action: 'queryer',
            params: [mainId, woId],
            type: 'SIE.Web.MES.BarcodeProcesses.BarcodeProcessDataQueryer',
            token: view.token,
            success: function success(res) {
                if (res.Success) {
                    SIE.Msg.showMessage("同步工单工序清单成功".t());
                    view._parent.reloadData();
                }
            }
        });
    }
})