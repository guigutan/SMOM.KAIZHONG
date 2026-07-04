SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.TaskFinishOrOpenCommand', {
    meta: { text: "完工/打开", group: "edit", iconCls: "icon-ArrowLeftRight icon-blue" },
    canExecute: function (view) {
        if (view && view.getSelection().length > 0 && view.getSelection().all(p => p.getTaskStatus() == 30 || p.getTaskStatus() == 60))
            return true;
        return false;
    },
    execute: function (view, source) {
        var me = this;
        view.execute({
            data: view.getSelectionIds(),
            success: function (res) {
                if (res.Success) {
                    view.reloadData();
                }
            }
        })
    }
});