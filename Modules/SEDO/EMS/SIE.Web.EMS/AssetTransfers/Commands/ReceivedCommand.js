SIE.defineCommand('SIE.Web.EMS.AssetTransfers.Commands.ReceivedCommand', {
    meta: { text: "接收", group: "edit", iconCls: "icon-Import icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ApprovalStatus !== 40 || model.data.TransferStatus != 20) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("接收成功!".t());
                view.reloadData();
            }
        });
    }
});