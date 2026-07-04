SIE.defineCommand('SIE.Web.EMS.InventoryPlans.Commands.CancelInventoryPlanCommand', {
    meta: { text: "撤回", group: "edit", iconCls: "icon-FileReturn icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.ApprovalStatus !== 20) return false;
        if (p.data.CloseRemark != "") return false;
        return true;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("撤回成功!".t());
                view.reloadData();
            }
        });
    }
});