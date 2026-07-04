SIE.defineCommand('SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.ForcedShutdownCommand', {
    meta: { text: "关闭", group: "edit", iconCls: "icon-CloseView icon-red" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {

            if (_golble_use_approval != null) {
                if ((model.data.ApprovalStatus !== 10 && model.data.ApprovalStatus !== 50) || model.data.Close == 1) {
                    res = false;
                    return false;
                }
            } else {
                if ((model.data.DemandState !== 5 && model.data.DemandState != 0) || model.data.Close == 1) {
                    res = false;
                    return false;
                }
            }
        });
        return res;
    },
    execute: function (view, source) {
        var me = this;
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定关闭选择的{0}条数据吗？'.t(), sel.length), function () {
            var selectModels = view.getSelection();
            var selectIds = view.getSelectionIds(selectModels);
            var postdata = {
                ApprovalResult: 0,
                Remark: "",
            };
            view.execute({
                data: postdata,
                withIds: true,
                selectIds: selectIds,
                success: function (res) {
                    SIE.Msg.showMessage("关单成功!".t());
                    view.reloadData();
                }
            });
        });
    }
});