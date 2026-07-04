SIE.defineCommand('SIE.Web.EMS.AssetRequisitions.Commands.CancelAssetRequisitionCommand', {
    meta: { text: "撤回", group: "edit", iconCls: "icon-FileReturn icon-blue" },
    canExecute: function (view) {

        if (view.hasSelectedEntities()) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getApprovalStatus() != 20) {
                    flag = false;
                    return false;
                }
            });
            return flag;
        }
        else {
            return false;
        }
    },
    canVisible: function (view, source) {
        var configValue = CRT.Context.PageContext.getContext('AssetRequisitionConfig');
        if (!configValue.EnableAudit) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        SIE.Msg.askQuestion("是否撤回？".t(), function () {
            view.execute({
                withIds: true,
                selectIds: selectIds,
                success: function (res) {
                    SIE.Msg.showMessage("撤回成功!".t());
                    view.reloadData();
                }
            });
        });
    }
});