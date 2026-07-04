SIE.defineCommand('SIE.Web.EMS.ViceTransfers.Commands.ExecuteTransferCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "执行", group: "edit", iconCls: "icon-PaperPlane icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if ((p.data.TransferStatus == 10 || p.data.TransferStatus == 30) && p.data.ApprovalStatus == 40) { return true; }
        else { return false; }
    },
    showView: function (editEntity) {
        //校验用户有来源仓库的权限，否则不能执行，报错：用户没有来源仓库权限，不能执行
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetWarehouseAvailable',
            params: [editEntity.getWarehouseId()],
            action: 'queryer',
            type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
            token: me.view.token,
            success: function (res) {
                if (res.Result != null) {
                    debugger;
                    if (!res.Result) {
                        SIE.Msg.showMessage("当前用户没有来源仓库权限，不能执行!".t());
                        return;
                    }
                    me.addPage({
                        entityType: me.view.model,
                        recordId: editEntity.getId(),
                        title: me.getEditViewTitle(editEntity),
                        isDetail: true,
                        isNew: true,
                        viewGroup: editEntity.getViceAssetObject() == 20 ? "ExecuteFixtureView" : "ExecuteSparePartView",
                        params: {
                        }
                    });
                }
            }
        });
    }
});