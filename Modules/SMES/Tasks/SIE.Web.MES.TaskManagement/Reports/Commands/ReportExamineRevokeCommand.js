SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.Commands.ReportExamineRevokeCommand', {
    meta: { text: "报工驳回", group: "edit", iconCls: "icon-FileReturn icon-blue" },
    canExecute: function (view) {
        var entitys = view.getSelection();
        if (entitys == null) {
             return false;
        }
        if (entitys.length == 0) {
            return false;
        }
        var flag = true;
        entitys.forEach(item => {
            if (item.getExamineState() !== 0) {
                flag = false;
            }
        });
        return flag;
    },
    execute: function (view, source) {
        var entityIds = view.getSelectionIds();
        SIE.Msg.wait("正在报工驳回......".t());
        view.execute({
            withIds: true,
            selectIds: entityIds,
            success: function (res) { //回调
                if (res.Result) {
                    SIE.Msg.showInstantMessage("报工驳回成功".t());
                    view.reloadData();
                }
            }
        })
    }
});