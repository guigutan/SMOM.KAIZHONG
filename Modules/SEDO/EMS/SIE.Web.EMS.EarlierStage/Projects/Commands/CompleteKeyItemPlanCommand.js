SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.CompleteKeyItemPlanCommand', {
    meta: { text: "完成", group: "edit", iconCls: "icon-ClipboardPaperCheck icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) return false;
        if (p.data.WorkStatus !== 20) return false;
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