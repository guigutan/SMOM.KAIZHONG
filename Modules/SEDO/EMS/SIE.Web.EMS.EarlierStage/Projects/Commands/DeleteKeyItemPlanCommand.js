SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.DeleteKeyItemPlanCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.WorkStatus == 20 || model.data.WorkStatus == 30) {
                res = false;
                return false;
            }
        });
        return res;
    }
});