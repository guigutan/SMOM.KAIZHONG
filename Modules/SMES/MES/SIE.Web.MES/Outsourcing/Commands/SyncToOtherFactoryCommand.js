SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.SyncToOtherFactoryCommand', {
    meta: { text: "同步到其他工厂", group: "edit", iconCls: "icon-Sync icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length <= 0) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);

        SIE.Msg.wait("正在同步数据，请稍等...".t());
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("同步成功!".t());
                CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
                view._parent.reloadData();
            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
            }
        });
    }
});
