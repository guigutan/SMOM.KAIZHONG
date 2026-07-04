SIE.defineCommand('SIE.Web.MES.TaskManagement.SchedulingInfs.Commands.SchedulingInfValidCommand', {
    meta: { text: "排程校验", group: "business", iconCls: "icon-TextQuality icon-blue" },

    canExecute: function (view) {
        if (view && view.getSelection() && view.getSelection().length > 0 && view.getSelection().all(p => p.getIsCheck() == false || p.getIsCheck() == null))
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
                }
            }
        });
    }

});