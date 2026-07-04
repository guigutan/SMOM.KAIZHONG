SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Records.Commands.MaintainPlanDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length <= 0) {
            return false;
        }
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ExeState != 0) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？确认后直接删除！'.t(), sel.length), function () {
            view.execute({
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) {
                    view.removeSelection();
                    view.setCurrent(null, true);
                    view.reloadData();
                },
            });
        });
    }
});