SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.PlanStartCommand', {
    meta: { text: "开始", group: "edit", iconCls: "icon-Play icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0)
            return false;
        var parent = view._parent.getCurrent();
        if (parent == null) {
            return false;
        }
        if (parent.data.ApprovalStatus !== 40) {
            return false;
        }
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.WorkStatus !== 10) {
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
                SIE.Msg.showMessage("开始成功!".t());
                view._parent.reloadData();
            }
        });
    }
});