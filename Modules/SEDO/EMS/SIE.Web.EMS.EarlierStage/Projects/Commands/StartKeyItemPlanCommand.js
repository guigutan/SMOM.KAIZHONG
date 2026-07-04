SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.StartKeyItemPlanCommand', {
    meta: { text: "开始", group: "edit", iconCls: "icon-Play icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.WorkStatus !== 10) return false;
        return true;
    },
    execute: function (view, source) {
        var p = view.getCurrent();
        view.execute({
            withIds: true,
            selectIds: [p.getId()],
            success: function (res) {
                SIE.Msg.showMessage("提交成功!".t());
                view.reloadData();
            }
        });
    }
});