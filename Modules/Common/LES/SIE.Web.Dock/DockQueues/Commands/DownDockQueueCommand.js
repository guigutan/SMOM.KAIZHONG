SIE.defineCommand('SIE.Web.Dock.DockQueues.Commands.DownDockQueueCommand', {
    meta: { text: "降级", group: "edit", iconCls: "iconfont icon-ArrowDownBold icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }

        var curData = view.getCurrent();
        if (curData == null) {
            return false;
        }

        if (curData.getQueueState() != 0 || curData.getQueuePriority() == 0) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {
        view.execute({
            data: view.getSelectionIds(),
            success: function (res) {
                view.reloadData();
            }
        });
    }
});