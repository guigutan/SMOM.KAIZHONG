SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.PlanEndCommand', {
    meta: { text: "完成", group: "edit", iconCls: "icon-ClipboardPaperCheck icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.WorkStatus !== 20) {
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
                SIE.Msg.showMessage("完成成功!".t());
                view._parent.reloadData();
            }
        });
    }
});