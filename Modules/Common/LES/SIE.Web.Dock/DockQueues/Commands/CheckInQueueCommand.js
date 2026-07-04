SIE.defineCommand('SIE.Web.Dock.DockQueues.Commands.CheckInQueueCommand', {
    meta: { text: "签到", group: "edit" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        var sel = view.getSelection();
        if (sel.any(function (p) { return p.getQueueState() != 1 || p.getCheckInTime() != null; }))
            return false;

        return true;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确认签到选择的{0}条数据吗?'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) {
                    SIE.Msg.showInstantMessage("签到成功!".t());
                    view.reloadData();
                }
            });
        });
    }
});