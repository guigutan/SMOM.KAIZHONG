SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.GeneraDispatchTaskCommand', {
    meta: { text: "生成任务单", group: "edit", iconCls: "icon-AlignBottom icon-blue" },
    canExecute: function (view) {
        if (view != null && view.getSelection().length > 0 && view.getSelection().all(p => p.getState() != 2 && p.getState() != 3 && p.getIsPause() != 1))
            return true;
        return false;
    },
    execute: function (view) {
        var ids = view.getSelectionIds();
        SIE.Msg.wait('生成任务单中......'.t());
        view.execute({
            data: ids,
            success: function (res) {
                SIE.Msg.showInstantMessage('生成结束'.t());
                view.reloadData();
            }
        });
    }
});