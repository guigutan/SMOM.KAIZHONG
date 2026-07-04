SIE.defineCommand('SIE.Web.MES.TaskManagement.HeatTreatments.Commands.HeatTreatmentReportCommand', {
    meta: { text: "报工", group: "edit", iconCls: "icon-ClipboardPaperCheck icon-blue" },
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
            if (item.getOperationType() !== 2 || item.getIsReported() == true) {
                flag = false;
            }
        });
        return flag;
    },
    execute: function (view, source) {
        var entityIds = view.getSelectionIds();
        SIE.Msg.wait("正在报工......".t());
        view.execute({
            withIds: true,
            selectIds: entityIds,
            success: function (res) { //回调
                if (res.Result) {
                    SIE.Msg.showInstantMessage("报工成功".t());
                    view.reloadData();
                }
            }
        })
    }
})