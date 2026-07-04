SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.SubmitSparePartAcceptanceCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-Refuse icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;

        var result = true;
        SIE.each(selectModels, function (model) {
            //修改没有保存，不让提交
            if (model.isDirty() || model.isChildrenDirty()) {
                result = false;
            }

            if (model.data.ApprovalStatus !== 10 && model.data.ApprovalStatus !== 50) {
                result = false;
            }
        });

        return result;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("提交成功!".t());
                view.reloadData();
            }
        });
    }
});