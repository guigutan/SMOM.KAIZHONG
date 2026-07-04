SIE.defineCommand("SIE.Web.MES.TaskManagement.ProcessPrepareRecords.Commands.RecordDetailAllPassCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "全部通过", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        //if (view.getData() === null || view.getData().data == null || view.getData().data.items == null || view.getData().data.items.length == 0) {
        //    return false;
        //}
        ////当结果都会通过的时候，置灰
        //if (view.getData().data.items.all(p => p.getResult() == 0)) {
        //    return false;
        //}

        return true;
    },
    execute: function (view) {
        if (view != null && view.getData() != null && view.getData().data != null && view.getData().data.items != null && view.getData().data.items.length > 0) {
            //全都改为通过
            view.getData().data.items.where(p => p.getResult() != 0).forEach(p => p.setResult(0));
        }
    },
});
