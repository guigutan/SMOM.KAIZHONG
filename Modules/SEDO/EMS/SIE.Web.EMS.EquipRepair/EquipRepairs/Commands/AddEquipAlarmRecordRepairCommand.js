SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddEquipAlarmRecordRepairCommand', {
    meta: { text: "生成维修工单", group: "edit", iconCls: "icon-FileReturn icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.EquipRepairBillId !== 0 && model.data.EquipRepairBillId != null) {
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
                SIE.Msg.showMessage("生成成功!".t());
                view.reloadData();
            }
        });
    }
});