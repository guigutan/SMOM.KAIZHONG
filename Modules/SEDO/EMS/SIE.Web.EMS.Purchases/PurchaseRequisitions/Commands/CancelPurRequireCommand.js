SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.CancelPurRequireCommand', {
    meta: { text: "撤回", group: "edit", iconCls: "icon-FileReturn icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {            
            //待审核 PendingReview = 20,
            //审核中 UnderReview = 30,
            if (model.data.ApprovalStatus !== 20 && model.data.ApprovalStatus !== 30) {
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
                SIE.Msg.showMessage("撤回成功!".t());
                view.reloadData();
            }
        });
    }
});