SIE.defineCommand('SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.PromptlyDeleteCommand', {
    extend: 'SIE.Web.Core.Common.Commands.ImmediateDeleteCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ApprovalStatus !== 10 && model.data.ApprovalStatus!=50) {
                res = false;
                return false;
            }
            if (model.data.BillSourceType != 10) {
                res = false;
                return false;
            }
            if (model.data.Close == 1) {
                res = false;
                return false;
            }
        });
        return res;
    }
})