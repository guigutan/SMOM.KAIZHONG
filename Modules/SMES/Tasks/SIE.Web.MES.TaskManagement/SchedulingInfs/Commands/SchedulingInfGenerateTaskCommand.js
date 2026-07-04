SIE.defineCommand('SIE.Web.MES.TaskManagement.SchedulingInfs.Commands.SchedulingInfGenerateTaskCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "手动下发", group: "business", iconCls: "icon-ArrowDown icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式

    canExecute: function (view) {
        //当全部都校验通过了并且不是作废的，才可以下发
        if (view && view.getSelection() && view.getSelection().length > 0 && view.getSelection().all(p => p.getIsCheck() == true && (p.getIsCancel() == false || p.getIsCancel() == null)))
            return true;
        return false;
    },

    execute: function (view) {
        var ids = view.getSelectionIds();
        view.execute({
            data: ids,
            success: function (res) {
                if (res.Success) {
                    view.reloadData();
                    SIE.Msg.showMessage(res.Result);
                }
            }
        });
    }

});